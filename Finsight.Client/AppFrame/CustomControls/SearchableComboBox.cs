using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Finsight.Client.AppFrame.CustomControls
{
    [TemplatePart(Name = PartSearchBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PartClearButton, Type = typeof(ButtonBase))]
    public class SearchableComboBox : ComboBox
    {
        private const string PartSearchBox = "PART_SearchBox";
        private const string PartClearButton = "PART_ClearButton";

        private readonly BindingEvaluator _bindingEvaluator = new();

        private TextBox? _searchTextBox;
        private ButtonBase? _clearButton;
        private ICollectionView? _collectionView;
        private Predicate<object>? _baseFilter;
        private bool _ignoreSearchTextChanges;

        static SearchableComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SearchableComboBox),
                new FrameworkPropertyMetadata(typeof(SearchableComboBox)));
        }

        public SearchableComboBox()
        {
            Loaded += OnLoaded;
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(
                nameof(SearchText),
                typeof(string),
                typeof(SearchableComboBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSearchTextPropertyChanged));

        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        private static void OnSearchTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchableComboBox control)
            {
                control.OnSearchTextChanged();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_searchTextBox != null)
            {
                _searchTextBox.PreviewKeyDown -= SearchTextBoxOnPreviewKeyDown;
                _searchTextBox.GotKeyboardFocus -= SearchTextBoxOnGotKeyboardFocus;
            }

            _searchTextBox = GetTemplateChild(PartSearchBox) as TextBox;
            if (_searchTextBox != null)
            {
                _searchTextBox.PreviewKeyDown += SearchTextBoxOnPreviewKeyDown;
                _searchTextBox.GotKeyboardFocus += SearchTextBoxOnGotKeyboardFocus;
            }

            if (_clearButton != null)
            {
                _clearButton.Click -= ClearButtonOnClick;
            }

            _clearButton = GetTemplateChild(PartClearButton) as ButtonBase;
            if (_clearButton != null)
            {
                _clearButton.Click += ClearButtonOnClick;
            }

            UpdateSelectionState();
        }

        protected override void OnItemsSourceChanged(IEnumerable? oldValue, IEnumerable? newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            EnsureCollectionView();
            RefreshFilter();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (ItemsSource == null)
            {
                EnsureCollectionView();
                RefreshFilter();
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            UpdateSelectionState();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (!HasSelection && e.NewFocus == this)
            {
                FocusSearchBox();
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == DisplayMemberPathProperty)
            {
                RefreshFilter();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            EnsureCollectionView();
            // При инициализации сбрасываем выбор, чтобы не было выделенного элемента
            //SelectedItem = null;
            var idx = SelectedIndex;
            var item = SelectedItem;
            UpdateSelectionState();
        }

        private void OnSearchTextChanged()
        {
            if (_ignoreSearchTextChanges)
            {
                return;
            }

            RefreshFilter();

            if (!IsDropDownOpen && !string.IsNullOrEmpty(SearchText))
            {
                IsDropDownOpen = true;
            }
        }

        private void SearchTextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && !IsDropDownOpen)
            {
                IsDropDownOpen = true;
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                if (HasSelection)
                {
                    SetCurrentValue(SelectedItemProperty, null);
                    e.Handled = true;
                }
                else if (!string.IsNullOrEmpty(SearchText))
                {
                    _ignoreSearchTextChanges = true;
                    SetCurrentValue(SearchTextProperty, string.Empty);
                    _ignoreSearchTextChanges = false;
                    RefreshFilter();
                    e.Handled = true;
                }
            }
        }

        private void SearchTextBoxOnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!HasSelection)
            {
                _searchTextBox?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _searchTextBox.SelectAll();
                }), DispatcherPriority.Input);
            }
        }

        private void ClearButtonOnClick(object? sender, RoutedEventArgs e)
        {
            _ignoreSearchTextChanges = true;
            SetCurrentValue(SearchTextProperty, string.Empty);
            _ignoreSearchTextChanges = false;

            if (SelectedItem != null)
            {
                SetCurrentValue(SelectedItemProperty, null);
            }
            else
            {
                RefreshFilter();
            }

            FocusSearchBox();
        }

        private void UpdateSelectionState()
        {
            var hasSelection = HasSelection;

            _ignoreSearchTextChanges = true;
            if (hasSelection && SelectedIndex != -1)
                SetCurrentValue(SearchTextProperty, GetItemDisplayText(SelectedItem));
            else
                SetCurrentValue(SearchTextProperty, string.Empty);
            _ignoreSearchTextChanges = false;

            if (_searchTextBox != null)
            {
                _searchTextBox.IsReadOnly = hasSelection;
                if (!hasSelection)
                {
                    _searchTextBox.CaretIndex = _searchTextBox.Text.Length;
                }
            }

            if (_clearButton != null)
            {
                _clearButton.Visibility = hasSelection ? Visibility.Visible : Visibility.Collapsed;
                _clearButton.IsHitTestVisible = hasSelection;
            }

            if (hasSelection)
            {
                IsDropDownOpen = false;
            }

            RefreshFilter();
        }

        private void EnsureCollectionView()
        {
            var view = ItemsSource != null
                ? CollectionViewSource.GetDefaultView(ItemsSource)
                : CollectionViewSource.GetDefaultView(Items);

            if (ReferenceEquals(_collectionView, view))
            {
                return;
            }

            if (_collectionView != null)
            {
                _collectionView.Filter = _baseFilter;
            }

            _collectionView = view;

            if (_collectionView != null)
            {
                _baseFilter = _collectionView.Filter;
                _collectionView.Filter = CombinedFilter;
            }
            else
            {
                _baseFilter = null;
            }
        }

        private bool CombinedFilter(object obj)
        {
            if (_baseFilter != null && !_baseFilter(obj))
            {
                return false;
            }

            return FilterPredicate(obj);
        }

        private void RefreshFilter()
        {
            EnsureCollectionView();
            _collectionView?.Refresh();
        }

        private bool FilterPredicate(object obj)
        {
            if (HasSelection)
            {
                return true;
            }

            var filter = SearchText;
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            var displayText = GetItemDisplayText(obj);
            return displayText.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private string GetItemDisplayText(object? item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(DisplayMemberPath))
            {
                return _bindingEvaluator.Evaluate(item, DisplayMemberPath);
            }

            return item.ToString() ?? string.Empty;
        }

        private bool HasSelection => SelectedItem != null;

        private void FocusSearchBox()
        {
            if (_searchTextBox == null)
            {
                return;
            }

            _searchTextBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                _searchTextBox.Focus();
                _searchTextBox.CaretIndex = _searchTextBox.Text.Length;
            }), DispatcherPriority.Input);
        }

        private sealed class BindingEvaluator : DependencyObject
        {
            public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register(nameof(Value), typeof(object), typeof(BindingEvaluator));

            public object? Value
            {
                get => GetValue(ValueProperty);
                set => SetValue(ValueProperty, value);
            }

            public string Evaluate(object source, string path)
            {
                var binding = new Binding(path) { Source = source };
                BindingOperations.SetBinding(this, ValueProperty, binding);
                var result = Value?.ToString() ?? string.Empty;
                BindingOperations.ClearBinding(this, ValueProperty);
                Value = null;
                return result;
            }
        }
    }
}
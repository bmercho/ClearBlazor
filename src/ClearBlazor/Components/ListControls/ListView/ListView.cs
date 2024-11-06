using ClearBlazorInternal;
using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// Displays a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.
    /// </summary>
    public class ListView<TItem> : ListViewBase<TItem>
        where TItem : ListItem
    {
        /// <summary>
        /// The template for rendering each row.
        /// The item is passed to each child for customization of the row
        /// </summary>
        [Parameter]
        public required RenderFragment<TItem>? RowTemplate { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            _rowTemplate = RowTemplate;
            _showHeader = false;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CC.Core.CoreViewModelAndDTOs;

namespace CC.Core.Html.Grid
{
    public interface IGrid<T> where T : IGridEnabledClass
    {
        void AddColumnModifications(Action<IGridColumn, T> modification);
        GridDefinition GetGridDefinition(string url);
        GridItemsViewModel GetGridItemsViewModel(GridItemsDetail pageDetail, IEnumerable<T> items);
    }

    public abstract class Grid<T> : IGrid<T> where T : IGridEnabledClass
    {
        protected readonly IGridBuilder<T> GridBuilder;
        private IList<Action<IGridColumn, T>> _modifications;

        protected Grid(IGridBuilder<T> gridBuilder)
        {
            GridBuilder = gridBuilder;
            _modifications = new List<Action<IGridColumn, T>>();
        }

        private IList<IDictionary<string, string>> GetGridColumns()
        {
            return GridBuilder.ToGridColumns();
        }

        private IEnumerable GetGridRows(IEnumerable rawResults)
        {
            foreach (T x in rawResults)
            {
                yield return new GridRow { id = x.EntityId, cell = GridBuilder.ToGridRow(x,_modifications) };
            }
        }

        public string GetDefaultSortColumnName()
        {
            var column = GridBuilder.columns.FirstOrDefault(
                x => x.Properties.Any(y => y.Key == GridColumnProperties.sortColumn.ToString()));
            return column == null ? string.Empty : column.Properties[GridColumnProperties.header.ToString()];
        }

        public void AddColumnModifications(Action<IGridColumn, T> modification)
        {
            _modifications.Add(modification);
        }

        public GridDefinition GetGridDefinition(string url)
        {
            return new GridDefinition
            {
                Url = url,
                Columns = BuildGrid().GetGridColumns()
            };
        }

        public GridItemsViewModel GetGridItemsViewModel(GridItemsDetail pageDetail, IEnumerable<T> items)
        {
            var model = new GridItemsViewModel
            {
                items = BuildGrid().GetGridRows(items),
                page = pageDetail.Page,
                records = pageDetail.TotalRows,
                total = pageDetail.TotalPages
            };
            return model;
        }

        protected virtual Grid<T> BuildGrid()
        {
            return this;
        }
    }

    public interface IGridEnabledClass
    {
        Guid EntityId { get; set; }
    }

    public class GridRow
    {
        public Guid id { get; set; }
        public string[] cell { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using CC.Core.CoreViewModelAndDTOs;
using CC.Core.Domain;
using CC.Core.DomainTools;

namespace CC.Core.Services
{
    public interface IUpdateCollectionService
    {
        void UpdateCollectionDetails<ENTITY>(IEnumerable<ENTITY> origional,
                    IEnumerable<ENTITY> newItems,
                    Action<ENTITY> addEntity,
                    Action<ENTITY> removeEntity) where ENTITY : Entity;

        void Update<ENTITY>(IEnumerable<ENTITY> origional,
                            TokenInputViewModel tokenInputViewModel,
                            Action<ENTITY> addEntity,
                            Action<ENTITY> removeEntity,
            Func<ENTITY, ENTITY, bool> comparer = null) where ENTITY : Entity, IPersistableObject;


    }

    public class UpdateCollectionService : IUpdateCollectionService
    {
        private readonly IRepository _repository;

        public UpdateCollectionService(IRepository repository)
        {
            _repository = repository;
        }

        public void UpdateCollectionDetails<ENTITY>(IEnumerable<ENTITY> origional,
            IEnumerable<ENTITY> newItems,
            Action<ENTITY> addEntity,
            Action<ENTITY> removeEntity) where ENTITY : Entity
        {
            var remove = new List<ENTITY>();
            origional.ForEachItem(x =>
            {
                var newItem = newItems.FirstOrDefault(i => i.EntityId == x.EntityId);
                if (newItem == null)
                {
                    remove.Add(x);
                }
                else
                {
                    x.UpdateSelf(newItem);
                }
            });
            remove.ForEachItem(removeEntity);
            newItems.ForEachItem(x =>
            {
                if (!origional.Contains(x))
                {
                    addEntity(x);
                }
            });
        }

        public void Update<ENTITY>(IEnumerable<ENTITY> origional,
            TokenInputViewModel tokenInputViewModel,
            Action<ENTITY> addEntity,
            Action<ENTITY> removeEntity,
            Func<ENTITY, ENTITY, bool> comparer = null) where ENTITY : Entity, IPersistableObject
        {
            if (comparer == null)
            {
                comparer = (entity, entity1) => entity1 != null && entity.EntityId == entity1.EntityId;
            }
            var newItems = new List<ENTITY>();
            if (tokenInputViewModel != null && tokenInputViewModel.selectedItems != null)
            { tokenInputViewModel.selectedItems.ForEachItem(x => newItems.Add(_repository.Find<ENTITY>(Int32.Parse(x.id)))); }

            var remove = new List<ENTITY>();
            if (newItems.Any())
            {
                if (origional.Any())
                {
                    var enumerable = origional.Where(x =>
                                                         {
                                                             var newItem = newItems.FirstOrDefault(i => i.EntityId == x.EntityId);
                                                             return newItem == null || !comparer(x, newItem);
                                                         });
                    enumerable.ForEachItem(remove.Add);
                }
            }
            else
            {
                remove = origional.ToList();
            }
            remove.ForEachItem(removeEntity);
            newItems.ForEachItem(x =>
            {
                if (!origional.Contains(x))
                {
                    addEntity(x);
                }
            });
        }
    }
}
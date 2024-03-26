using System;
using System.Collections.Generic;

namespace LinqSharp.EFCore;

public class ModelCreator
{
    public readonly Dictionary<Type, EntityBuilder> _builderDict = new();

    public class EntityBuilder
    {
        public string[] Keys { get; private set; }
        public readonly List<RelatedInfo> RelatedList = new();

        public EntityBuilder HasKeys(params string[] keys)
        {
            Keys = keys;
            return this;
        }

        public EntityBuilder OneToOne(string related, string navigation, RelatedBehavior behavior)
        {
            var info = new RelatedInfo
            {
                Action = RelatedAction.OneToOne,
                Related = related,
                Navigation = navigation,
                Behavior = behavior,
            };
            RelatedList.Add(info);
            return this;
        }

        public EntityBuilder OneToMany(string related, string navigation, RelatedBehavior behavior)
        {
            var info = new RelatedInfo
            {
                Action = RelatedAction.OneToMany,
                Related = related,
                Navigation = navigation,
                Behavior = behavior,
            };
            RelatedList.Add(info);
            return this;
        }

        public EntityBuilder ManyToOne(string related, string navigation, RelatedBehavior behavior)
        {
            var info = new RelatedInfo
            {
                Action = RelatedAction.ManyToOne,
                Related = related,
                Navigation = navigation,
                Behavior = behavior,
            };
            RelatedList.Add(info);
            return this;
        }

    }

    public EntityBuilder Entity<TEntity>()
    {
        var type = typeof(TEntity);
        if (!_builderDict.ContainsKey(type))
        {
            _builderDict[type] = new EntityBuilder();
        }

        return _builderDict[type];
    }

}

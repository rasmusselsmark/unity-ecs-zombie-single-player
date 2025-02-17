using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

public static class Utils
{
    public static Entity TriggerEventHasComponent<T>(TriggerEvent triggerEvent, ComponentLookup<T> lookup)
        where T : unmanaged, IComponentData
    {
        // there is no guarantee on order of EntityA and EntityB, so we have to check for both
        if (lookup.HasComponent(triggerEvent.EntityA))
        {
            return triggerEvent.EntityA;
        }

        if (lookup.HasComponent(triggerEvent.EntityB))
        {
            return triggerEvent.EntityB;
        }

        return Entity.Null;
    }

    public static Entity CollisionEventHasComponent<T>(CollisionEvent collisionEvent, ComponentLookup<T> lookup)
        where T : unmanaged, IComponentData
    {
        // there is no guarantee on order of EntityA and EntityB, so we have to check for both
        if (lookup.HasComponent(collisionEvent.EntityA))
        {
            return collisionEvent.EntityA;
        }

        if (lookup.HasComponent(collisionEvent.EntityB))
        {
            return collisionEvent.EntityB;
        }

        return Entity.Null;
    }

    public static void DeleteEntityWithChildren(Entity entity, EntityManager entityManager, EntityCommandBuffer ecb)
    {
        // first delete the children, since we've seen that Entities Graphics creates entities without Linked Entity Group
        if (!entityManager.HasComponent<LinkedEntityGroup>(entity))
        {
            var children = entityManager.GetBuffer<Child>(entity);
            foreach (var child in children)
            {
                ecb.DestroyEntity(child.Value);
            }
        }

        ecb.DestroyEntity(entity);
    }
}

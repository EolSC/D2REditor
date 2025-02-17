using Diablo2Editor;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Diablo2Editor
{

    public enum ComponentTypes
    {
        TransformDefinitionComponent,
        TerrainDefinitionComponent,
        ModelDefinitionComponent,
        ModelVariationDefinitionComponent,
        PhysicsBodyDefinitionComponent,
        WallTransparencyComponent,
        PrefabPlacementDefinitionComponent,
        VfxDefinitionComponent,
        TerrainDecalDefinitionComponent,
        TerrainStampDefinitionComponent,
        PointLightDefinitionComponent,
        SpotLightDefinitionComponent,
        AudioEnvironmentOverrideComponent,
        ModelPlatformTierComponent,
        DecalDefinitionComponent,
        UnitRootComponent,
        UnitPartComponent,
        SkeletonDefinitionComponent,
        OverrideModelDefinitionComponent,
        AudioEmitterComponent,
        TransformVariationDefinitionComponent,
        ObjectMountPointComponent,
        FadeTargetComponent,
        VisualDataBoxDefinitionComponent,
        PhysicsClothDefinitionComponent,
        PhysicsPartialRagdollDefinitionComponent,
        LevelHeightOffsetComponent,
        ChangeVisibilityOnDataEventComponent,
        DefaultAttachmentTransformComponent,
        ParallaxOffsetComponent,
    }

    public class ComponentFactory
    {
        static Dictionary<ComponentTypes, Type> componentTypes = new Dictionary<ComponentTypes, Type>
        {
            { ComponentTypes.TransformDefinitionComponent, typeof(TransformDefinitionComponent) },
            { ComponentTypes.TerrainDefinitionComponent, typeof(TerrainDefinitionComponent) },
            { ComponentTypes.ModelDefinitionComponent, typeof(ModelDefinitionComponent) },
            { ComponentTypes.ModelVariationDefinitionComponent, typeof(ModelVariationDefinitionComponent) },
            { ComponentTypes.PhysicsBodyDefinitionComponent, typeof(PhysicsBodyDefinitionComponent) },
            { ComponentTypes.WallTransparencyComponent, typeof(WallTransparencyComponent) },
            { ComponentTypes.PrefabPlacementDefinitionComponent, typeof(PrefabPlacementDefinitionComponent) },
            { ComponentTypes.VfxDefinitionComponent, typeof(VfxDefinitionComponent) },
            { ComponentTypes.TerrainDecalDefinitionComponent, typeof(TerrainDecalDefinitionComponent) },
            { ComponentTypes.TerrainStampDefinitionComponent, typeof(TerrainStampDefinitionComponent) },
            { ComponentTypes.PointLightDefinitionComponent, typeof(PointLightDefinitionComponent) },
            { ComponentTypes.SpotLightDefinitionComponent, typeof(SpotLightDefinitionComponent) },
            { ComponentTypes.AudioEnvironmentOverrideComponent, typeof(AudioEnvironmentOverrideComponent) },
            { ComponentTypes.ModelPlatformTierComponent, typeof(ModelPlatformTierComponent) },
            { ComponentTypes.DecalDefinitionComponent, typeof(DecalDefinitionComponent) },
            { ComponentTypes.UnitRootComponent, typeof(UnitRootComponent) },
            { ComponentTypes.UnitPartComponent, typeof(UnitPartComponent) },
            { ComponentTypes.SkeletonDefinitionComponent, typeof(SkeletonDefinitionComponent) },
            { ComponentTypes.OverrideModelDefinitionComponent, typeof(OverrideModelDefinitionComponent) },
            { ComponentTypes.AudioEmitterComponent, typeof(AudioEmitterComponent) },
            { ComponentTypes.TransformVariationDefinitionComponent, typeof(TransformVariationDefinitionComponent) },
            { ComponentTypes.ObjectMountPointComponent, typeof(ObjectMountPointComponent) },
            { ComponentTypes.FadeTargetComponent, typeof(FadeTargetComponent) },
            { ComponentTypes.VisualDataBoxDefinitionComponent, typeof(VisualDataBoxDefinitionComponent) },
            { ComponentTypes.PhysicsClothDefinitionComponent, typeof(PhysicsClothDefinitionComponent) },
            { ComponentTypes.PhysicsPartialRagdollDefinitionComponent, typeof(PhysicsPartialRagdollDefinitionComponent) },
            { ComponentTypes.LevelHeightOffsetComponent, typeof(LevelHeightOffsetComponent) },
            { ComponentTypes.ChangeVisibilityOnDataEventComponent, typeof(ChangeVisibilityOnDataEventComponent) },
            { ComponentTypes.DefaultAttachmentTransformComponent, typeof(DefaultAttachmentTransformComponent) },
            { ComponentTypes.ParallaxOffsetComponent, typeof(ParallaxOffsetComponent) },
        };
             
        public static LevelEntityComponent CreateComponentByType(string type, GameObject gameObject, ref bool componentValid)
        {
            object componentType;
            if (Enum.TryParse(typeof(ComponentTypes), type, out componentType))
            {
                ComponentTypes parsedType = (ComponentTypes)componentType;
                Type typeToCreate = componentTypes[parsedType];
                componentValid = true;
                return gameObject.AddComponent(typeToCreate) as LevelEntityComponent;
            }
            componentValid = false;
            return gameObject.AddComponent<LevelComponentUnknown>();
        }
    }
}
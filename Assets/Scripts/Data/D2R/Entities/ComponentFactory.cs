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
        SkeletonComponent,
        OverrideModelDefinitionComponent,
        AudioEmitterComponent,
        TransformVariationDefinitionComponent,
        ObjectMountPointComponent,
        FadeTargetComponent,
        VisualDataBoxDefinitionComponent,
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
            { ComponentTypes.SkeletonComponent, typeof(SkeletonComponent) },
            { ComponentTypes.OverrideModelDefinitionComponent, typeof(OverrideModelDefinitionComponent) },
            { ComponentTypes.AudioEmitterComponent, typeof(AudioEmitterComponent) },
            { ComponentTypes.TransformVariationDefinitionComponent, typeof(TransformVariationDefinitionComponent) },
            { ComponentTypes.ObjectMountPointComponent, typeof(ObjectMountPointComponent) },
            { ComponentTypes.FadeTargetComponent, typeof(FadeTargetComponent) },
            { ComponentTypes.VisualDataBoxDefinitionComponent, typeof(VisualDataBoxDefinitionComponent) },
        };
             
        public static LevelEntityComponent CreateComponentByType(string type, GameObject gameObject, bool checkMissingComponents)
        {
            object componentType;
            if (Enum.TryParse(typeof(ComponentTypes), type, out componentType))
            {
                ComponentTypes parsedType = (ComponentTypes)componentType;
                Type typeToCreate = componentTypes[parsedType];
                return gameObject.AddComponent(typeToCreate) as LevelEntityComponent;
            }
            if (checkMissingComponents)
            {
                Debug.LogError("Unknown component: " + type);
            }
            return gameObject.AddComponent<LevelComponentUnknown>();
        }
    }
}
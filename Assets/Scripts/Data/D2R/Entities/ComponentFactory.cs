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

        };
             
        public static LevelEntityComponent CreateComponentByType(string type, GameObject gameObject)
        {
            object componentType;
            if (Enum.TryParse(typeof(ComponentTypes), type, out componentType))
            {
                ComponentTypes parsedType = (ComponentTypes)componentType;
                Type typeToCreate = componentTypes[parsedType];
                return gameObject.AddComponent(typeToCreate) as LevelEntityComponent;
            }
            Debug.Log("Unknown component: " + type);
            return gameObject.AddComponent<LevelComponentUnknown>();
        }
    }
}
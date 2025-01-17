using UnityEngine;
namespace Diablo2Editor
{
    /*
     * Enum for possible types of resources.
     * Order of resources matter because of dependencies between them
     * Textures and Skeletons first, then models, then the others
     */
    public enum DependencyType
    {
        Textures,
        Skeletons,
        Particles, 
        Models,
        Animations,
        Physics,
        Json, 
        VariantData,
        ObjectEffects,
        Other
    }
}

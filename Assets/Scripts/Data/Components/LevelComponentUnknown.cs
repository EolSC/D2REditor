using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
    * Special type of component used in cases we try to load some component unknown to editor. 
    * Usually it's type error or some unsupported type
    */
    public class LevelComponentUnknown : LevelEntityComponent
    {

    }
}
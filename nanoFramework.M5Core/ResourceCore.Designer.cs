//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace nanoFramework.M5Stack
{
    
    internal partial class ResourceCore
    {
        private static System.Resources.ResourceManager manager;
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if ((ResourceCore.manager == null))
                {
                    ResourceCore.manager = new System.Resources.ResourceManager("nanoFramework.M5Core.ResourceCore", typeof(ResourceCore).Assembly);
                }
                return ResourceCore.manager;
            }
        }
        internal static nanoFramework.UI.Font GetFont(ResourceCore.FontResources id)
        {
            return ((nanoFramework.UI.Font)(nanoFramework.Runtime.Native.ResourceUtility.GetObject(ResourceManager, id)));
        }
        [System.SerializableAttribute()]
        internal enum FontResources : short
        {
            consolas_regular_16 = 10023,
        }
    }
}

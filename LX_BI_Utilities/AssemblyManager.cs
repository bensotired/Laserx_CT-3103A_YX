using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LX_BurnInSolution.Utilities
{
    public static class AssemblyManager
    {
        static Dictionary<string, Assembly> Assemblies { get; set; }
        static Dictionary<string, Assembly> PreLoad_Assemblies { get; set; }
 
    
     
        public static void Initialize(List<string> preLoadDllList )
        {
            Assemblies = new Dictionary<string, Assembly>();
            PreLoad_Assemblies = new Dictionary<string, Assembly>();
            var appAss = Assembly.GetEntryAssembly();
            var appRefAssNames = appAss.GetReferencedAssemblies();

            foreach (var assName in appRefAssNames)
            {
                var ass = Assembly.Load(assName);
                Assemblies.Add(ass.FullName.Split(',')[0], ass);
            }
            if (preLoadDllList != null)
            {
                PreLoad_Assemblies.Clear();
                foreach (var dllpath in preLoadDllList)
                {

                    try
                    {
                        var dllAssembly = Assembly.LoadFrom(dllpath);
                        var dllkey = dllAssembly.FullName.Split(',')[0];
                        if (Assemblies.ContainsKey(dllkey))
                        {
                            continue;
                        }
                        else
                        {
                            Assemblies.Add(dllkey, dllAssembly);
                            PreLoad_Assemblies.Add(dllkey, dllAssembly);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debugger.Log(0, "AssemblyManager.Initialize", ex.StackTrace);
                        throw ex;
                    }
                }
            }
        }
 
        /// <summary>
        /// silm way to create TInstance
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <returns></returns>
        public static TInstance CreateInstanceSlim<TInstance>() where TInstance : class
        {
            var dataType = typeof(TInstance);
            var dataObject = Activator.CreateInstance(dataType);
            var data = dataObject as TInstance;
            return data;
        }
        /// <summary>
        ///  silm way to create TInstance with constructor Params
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="constructorParams"></param>
        /// <returns></returns>
        public static TInstance CreateInstanceSlim<TInstance>(params object[] constructorParams) where TInstance : class
        {
            var dataType = typeof(TInstance);
            var dataObject = Activator.CreateInstance(dataType, constructorParams);
            var data = dataObject as TInstance;
            return data;
        }
        //public static TInstance CreateInstance<TInstance>(string className)
        //{
        //    return CreateInstance<TInstance>(className);
        //}

        public static TInstance CreateInstance<TInstance>(string className, params object[] constructorParams)
        {
            Type instanceType = null;

            //foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (Assembly assembly in Assemblies.Values)
            {
                instanceType = assembly.GetType(className, false);
                if (instanceType != null)
                {
                    break;
                }
            }
            if (instanceType == null)
            {
                throw new Exception(string.Format("Can not create [{0}],class not found.", className));
            }
            return CreateInstance<TInstance>(className, instanceType, constructorParams);
        }

        public static TInstance CreateInstance<TInstance>(string className, Type instanceType, params object[] constructorParams)
        {
            if (instanceType.IsAbstract || instanceType.IsInterface)
            {
                throw new Exception(string.Format("Can not create an instance for [{0}],it is an abstract class or an interface.", className));
            }
            object workerObject = null;
            TInstance workerInstance = default(TInstance);
            try
            {
                workerObject = Activator.CreateInstance(instanceType, constructorParams);
                workerInstance = (TInstance)((object)workerObject);
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Can not create an instance for [{0}].Exception:[{1}].", className, ex.Message);
                //LogManager.Log(errMsg);
                throw new Exception(errMsg, ex);
            }
            return workerInstance;
        }

        public static TInstance CreateInstanceFromSourceDll<TInstance>(string className, Type sourceDllType, params object[] constructorParams)
        {
            //if (instanceType.IsAbstract || instanceType.IsInterface)
            //{
            //    throw new Exception(string.Format("Can not create an instance for [{0}],it is an abstract class or an interface.", className));
            //}
            object workerObject = null;

            var sourceTypes = sourceDllType.Assembly.GetTypes();
            Type instanceType = null;
            foreach (var subType in sourceTypes)
            {
                if (className == subType.FullName)
                {
                    instanceType = subType;
                }
            }

            TInstance workerInstance = default(TInstance);
            try
            {
                workerObject = Activator.CreateInstance(instanceType, constructorParams);
                workerInstance = (TInstance)((object)workerObject);
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Can not create an instance for [{0}].Exception:[{1}].", className, ex.Message);
                //LogManager.Log(errMsg);
                throw new Exception(errMsg, ex);
            }
            return workerInstance;
        }

        public static Type GetTypeFromClass(string className)
        {
            Type instanceType = null;
            //foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) 
            foreach (Assembly assembly in Assemblies.Values)
            {
                instanceType = assembly.GetType(className, false);
          
                if (instanceType != null)
                {
                    break;
                }
            }
            if (instanceType == null)
            {
                throw new Exception(string.Format("Can not create [{0}],class not found.", className));
            }
            return instanceType;
        }
        public static Type GetTypeFromClassInPreLoadDlls(string className)
        {
            Type instanceType = null;
            foreach (var preloadDllKey in PreLoad_Assemblies.Keys)
            {
                if (Assemblies.ContainsKey(preloadDllKey))
                {
                    var preLoadAssembly = Assemblies[preloadDllKey];
                    instanceType = preLoadAssembly.GetType(className, false);

                    if (instanceType != null)
                    {
                        //带namespace的类找到了
                        return instanceType;
                    }
                    else
                    {
                        //带namespace的类没有找到 找preloaddll里面的类 
                        foreach (var defType in preLoadAssembly.DefinedTypes)
                        {
                            if (defType.Name == className)
                            {
                                //找到了很好s
                                instanceType = GetTypeFromClass(defType.FullName);
                                return instanceType;
                            }
                        }
                    }
                }
            }
            if (instanceType == null)
            {
                //怎么都没有 毁灭吧
                throw new Exception(string.Format("Class [{0}] not found.", className));
            }
            return instanceType;
        }
        public static bool PreLoadDllsContainsClass(string className)
        {
            bool isClassFound = false;
            foreach (var preloadDllKey in PreLoad_Assemblies.Keys)
            {
                if (Assemblies.ContainsKey(preloadDllKey))
                {
                    var preLoadAssembly = Assemblies[preloadDllKey];
                    var instanceType = preLoadAssembly.GetType(className, false);

                    if (instanceType != null)
                    {
                        //带namespace的类找到了
                        isClassFound = true;
                    }
                    else
                    {
                        //带namespace的类没有找到 找preloaddll里面的类 
                        foreach (var defType in preLoadAssembly.DefinedTypes)
                        {
                            if (defType.Name == className)
                            {
                                //找到了很好s
                                instanceType = GetTypeFromClass(defType.FullName);
                                if (instanceType != null)
                                {
                                    //带namespace的类找到了
                                    isClassFound = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (isClassFound)
                    {
                        break;
                    }
                }
            }
            return isClassFound;
        }

        public static Type GetTypeFromClassInAppPluginDlls(string className)
        {
            Type instanceType = null;

            try
            {

          
            //先在小范围Preload dlls找
            foreach (var preLoadAssembly in PreLoad_Assemblies.Values)
            {

                instanceType = preLoadAssembly.GetType(className, false);

                if (instanceType != null)
                {
                    //带namespace的类找到了
                    return instanceType;
                }
                else
                {
                    //带namespace的类没有找到 找preloaddll里面的类 
                    foreach (var defType in preLoadAssembly.DefinedTypes)
                    {
                        if (defType.Name == className)
                        {
                            //找到了很好
                            instanceType = GetTypeFromClass(defType.FullName);
                            return instanceType;
                        }
                    }
                }
            }

            //后在大范围全局dll找 
            foreach (var preLoadAssembly in Assemblies.Values)
            {

                instanceType = preLoadAssembly.GetType(className, false);

                if (instanceType != null)
                {
                    //带namespace的类找到了
                    return instanceType;
                }
                else
                {
                    //带namespace的类没有找到 找preloaddll里面的类 
                    foreach (var defType in preLoadAssembly.DefinedTypes)
                    {
                        if (defType.Name == className)
                        {
                            //找到了很好
                            instanceType = GetTypeFromClass(defType.FullName);
                            return instanceType;
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {

            }
            if (instanceType == null)
            {
                //怎么都没有 毁灭吧
                throw new Exception(string.Format("Class [{0}] not found.", className));
            }
            return instanceType;
        }
        public static List<Type> GetAssignableTypesFromPreLoadDlls(Type baseType)
        {
            List<Type> childrenClassObjs = new List<Type>();

            try
            {
                foreach (var preloadDllKey in PreLoad_Assemblies.Keys)
                {
                    if (Assemblies.ContainsKey(preloadDllKey))
                    {
                        var sourceTypes = Assemblies[preloadDllKey].GetTypes();

                        foreach (var subType in sourceTypes)
                        {
                            //subType从baseType派生
                            if (baseType.IsAssignableFrom(subType) &&
                                // 且  不是baseType自身
                                baseType.Equals(subType) == false)
                            {

                                childrenClassObjs.Add(subType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return childrenClassObjs;
        }
        public static List<object> GetAssignableInstances(Type sourceDllType, Type baseType)
        {
            //在源dll找出包含的所有类型
            var sourceTypes = sourceDllType.Assembly.GetTypes();
            List<object> childrenClassObjs = new List<object>();
            foreach (var subType in sourceTypes)
            {
                    //subType从baseType派生
                if (baseType.IsAssignableFrom(subType) && 
                    // 且  不是baseType自身
                    baseType.Equals(subType) == false)  
                {
                    object subObj = Activator.CreateInstance(subType );
                    childrenClassObjs.Add(subObj);
                }
            }
            return childrenClassObjs;
        }
        /// <summary>
        /// 提取指定父类在dll类集合中的所有子类并实例化
        /// </summary>
        /// <typeparam name="OutType"></typeparam>
        /// <param name="sourceDllType"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static List<OutType>GetAssignableInstances<OutType>(Type sourceDllType, Type baseType)
        {
            List<OutType> childrenClassObjs = new List<OutType>();

            try
            {
                //在源dll找出包含的所有类型
                var sourceTypes = sourceDllType.Assembly.GetTypes();

                foreach (var subType in sourceTypes)
                {
                    //subType从baseType派生
                    if (baseType.IsAssignableFrom(subType) &&
                        // 且  不是baseType自身
                        baseType.Equals(subType) == false)
                    {
                        var subObj = (OutType)Activator.CreateInstance(subType);
                        childrenClassObjs.Add(subObj);
                    }
                }
            }
            catch (Exception ex)
            {

            
            }
        
            return childrenClassObjs;
        }
        public static List<Type> GetAssignableTypes (string sourceDllKey, Type baseType)
        {
            List<Type> childrenClassObjs = new List<Type>();

            try
            {
                //在源dll找出包含的所有类型
                if (Assemblies.ContainsKey(sourceDllKey))
                {
                    var sourceTypes = Assemblies[sourceDllKey].GetTypes();

                    foreach (var subType in sourceTypes)
                    {
                        //subType从baseType派生
                        if (baseType.IsAssignableFrom(subType) &&
                            // 且  不是baseType自身
                            baseType.Equals(subType) == false)
                        {
                          
                            childrenClassObjs.Add(subType);
                        }
                    }
                }
                else
                {

                }
              
            }
            catch (Exception ex)
            {


            }

            return childrenClassObjs;
        }
    }
}

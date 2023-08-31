using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEngine;
using static CommunicationEvents;


// I would go for static virtual methods, but C#9 does not allow me...
// static methods cannot be overwritten -> virtual
public interface IJSONsavable<T> where T : IJSONsavable<T>, new()
{
    // stand-in for non static methods
    public static readonly IJSONsavable<T> Instance = new T();

    public static readonly FieldInfo[] JsonSaveableFields =
    #region one-time-initialisation
         typeof(T)
        .GetFields(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Static)
        .Where((field)
            => !field.GetCustomAttributes().Any((attribute)
                => attribute.GetType() == typeof(JsonIgnoreAttribute))
            && field.FieldType.GetInterfaces().Any((inter)
                => inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IJSONsavable<>)))
        .ToArray();
    #endregion one-time-initialisation

    public static readonly FieldInfo[] JsonAutoPreProcessFields =
    #region one-time-initialisation
         typeof(T)
        .GetFields(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Static)
        .Where((field)
            => !field.GetCustomAttributes().Any((attribute)
                => attribute.GetType() == typeof(JsonIgnoreAttribute))
            && field.GetCustomAttributes().Any((attribute)
                => attribute.GetType() == typeof(JSONsavable.JsonAutoPreProcessAttribute))
            && field.FieldType.GetInterfaces().Any((inter)
                => inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IJSONsavable<>)))
        .ToArray();
    #endregion one-time-initialisation

    public static readonly FieldInfo[] JsonAutoPostProcessFields =
    #region one-time-initialisation
         typeof(T)
        .GetFields(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Static)
        .Where((field)
            => !field.GetCustomAttributes().Any((attribute)
                => attribute.GetType() == typeof(JsonIgnoreAttribute))
            && field.GetCustomAttributes().Any((attribute)
                => attribute.GetType() == typeof(JSONsavable.JsonAutoPostProcessAttribute))
            && field.FieldType.GetInterfaces().Any((inter)
                => inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IJSONsavable<>)))
        .ToArray();
    #endregion one-time-initialisation

    public static readonly FieldInfo[] JsonSeperateFields =
    #region one-time-initialisation
         typeof(T)
        .GetFields(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Static)
        .Where((field)
            => field.GetCustomAttributes().Any((attribute)
                => attribute.GetType() == typeof(JSONsavable.JsonSeparateAttribute))
            && field.FieldType.GetInterfaces().Any((inter)
                => inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IJSONsavable<>)))
        .ToArray();
    #endregion one-time-initialisation


    // TODO: this?
    public string name { get; set; }
    public string path { get; set; }
    protected static List<Directories>
        hierarchie = new List<Directories> { Directories.misc };

    #region OverridableMethods

    public virtual string _IJGetName(string name) => name;
    public virtual List<Directories> _IJGetHierarchie(List<Directories> hierarchie_base)
    {
        hierarchie_base ??= new List<Directories>();
        return hierarchie_base.Concat(hierarchie).ToList();
    }
    public virtual bool _IJGetRawObject(out T payload, string path) => JSONsavable.ReadFromJsonFile<T>(out payload, path);
    public virtual T _IJPreProcess(T payload) => payload;
    public virtual T _IJPostProcess(T payload) => payload;

    #endregion OverridableMethods


    #region MethodTemplates

    public bool store(List<Directories> hierarchie, string name, bool use_install_folder = false, bool overwrite = true, bool deep_store = true)
        => store(hierarchie, name, (T)this, use_install_folder, overwrite, deep_store);

    public static bool store(List<Directories> hierarchie, string name, T payload, bool use_install_folder = false, bool overwrite = true, bool deep_store = true)
    {
        var new_hierarchie =
            Instance._IJGetHierarchie(hierarchie);
        var new_name =
            Instance._IJGetName(name);

        string path = CreatePathToFile(out bool exists, new_name, "JSON", new_hierarchie, use_install_folder);
        if (exists && !overwrite)
            return false;

        // store fields decorated with JsonSeparateAttribute and implementing IJSONsavable<> separately
        if (deep_store
            && !store_children(hierarchie, name, payload, use_install_folder: false, overwrite, deep_store: true))
            return false;

        // store this
        string path_o = payload.path;
        payload.path = path;

        var new_payload =
            preprocess(payload);

        payload.path = path_o;

        JSONsavable.WriteToJsonFile(path, new_payload);
        return true;
    }

    public bool store_children(List<Directories> hierarchie, string name, bool use_install_folder = false, bool overwrite = true, bool deep_store = true)
        => store_children(hierarchie, name, (T)this, use_install_folder, overwrite, deep_store);

    public static bool store_children(List<Directories> hierarchie, string name, T payload, bool use_install_folder = false, bool overwrite = true, bool deep_store = true)
    {
        var new_hierarchie =
            Instance._IJGetHierarchie(hierarchie);
        var new_name =
            Instance._IJGetName(name);

        for ((int max_i, bool success) = (0, true); max_i < JsonSeperateFields.Count(); max_i++)
        {
            var field = JsonSeperateFields[max_i];
            dynamic save_me = field.GetValue(payload); // is S:IJSONsavable<S>

            Type interface_type = typeof(IJSONsavable<>).MakeGenericType(field.FieldType);
            Type[] store_args_type = new Type[] { typeof(List<Directories>), typeof(string), field.FieldType, typeof(bool), typeof(bool), typeof(bool) };
            object[] store_args = new object[] { new_hierarchie, new_name, save_me, use_install_folder, overwrite, deep_store };

            var method = interface_type.GetMethod("store", store_args_type);
            success &= (bool)method.Invoke(null, store_args);

            // in case of no success: delete it again
            if (!success)
            {
                delete_children(hierarchie, name, use_install_folder, JsonSeperateFields.Count() - max_i);
                return false;
            }
        }

        return true;
    }

    public static bool load_children(List<Directories> hierarchie, string name, ref T raw_payload, bool use_install_folder = false, bool deep_load = true, bool post_process = true)
    {
        var new_hierarchie =
            Instance._IJGetHierarchie(hierarchie);
        var new_name =
            Instance._IJGetName(name);

        bool success = true;
        for (int max_i = 0; max_i < JsonSeperateFields.Count(); max_i++)
        {
            var field = JsonSeperateFields[max_i];

            Type interface_type = typeof(IJSONsavable<>).MakeGenericType(field.FieldType);
            Type[] load_args_type = new Type[] { typeof(List<Directories>), typeof(string), field.FieldType.MakeByRefType(), typeof(bool), typeof(bool), typeof(bool) };
            object[] load_args = new object[] { new_hierarchie, new_name, null, use_install_folder, deep_load, post_process };

            var method = interface_type.GetMethod("load", BindingFlags.Public | BindingFlags.Static, null, load_args_type, null);
            bool success_i = (bool)method.Invoke(null, load_args);

            field.SetValue(raw_payload, success_i ? load_args[2] : Activator.CreateInstance(field.FieldType));
            success &= success_i;
        }

        return success;
    }

    public static bool load(List<Directories> hierarchie, string name, out T payload, bool use_install_folder = false, bool deep_load = true, bool post_process = true)
    {
        payload = default(T);
        bool success = true;

        var new_hierarchie =
            Instance._IJGetHierarchie(hierarchie);
        var new_name =
            Instance._IJGetName(name);

        string path = CreatePathToFile(out bool loadable, new_name, "JSON", new_hierarchie, use_install_folder);
        if (!loadable)
            return false;

        if (!Instance._IJGetRawObject(out T raw_payload, path))
            return false;
        raw_payload.name = new_name;

        // load fields decorated with JsonSeparateAttribute and implementing IJSONsavable<> separately
        if (deep_load
            && !load_children(hierarchie, name, ref raw_payload, false /*use_install_folder*/))
            success = false;

        payload = post_process
            ? postprocess(raw_payload)
            : raw_payload;

        return success;
    }

    public static T postprocess(T payload)
    {
        if (payload == null)
            return Instance._IJPostProcess(payload);

        for (int i = 0; i < JsonAutoPostProcessFields.Count(); i++)
        {
            var field = JsonAutoPostProcessFields[i];
            dynamic process_me = field.GetValue(payload); // is S:IJSONsavable<S>

            Type interface_type = typeof(IJSONsavable<>).MakeGenericType(field.FieldType);
            Type[] process_args_type = new Type[] { field.FieldType };
            object[] process_args = new object[] { process_me };

            var method = interface_type.GetMethod("postprocess", process_args_type);
            var processed = method.Invoke(null, process_args);

            field.SetValue(payload, processed);
        }

        return Instance._IJPostProcess(payload);
    }

    public static T preprocess(T payload)
    {
        if (payload == null)
            return Instance._IJPreProcess(payload);

        for (int i = 0; i < JsonAutoPreProcessFields.Count(); i++)
        {
            var field = JsonAutoPreProcessFields[i];
            dynamic process_me = field.GetValue(payload); // is S:IJSONsavable<S>

            Type interface_type = typeof(IJSONsavable<>).MakeGenericType(field.FieldType);
            Type[] process_args_type = new Type[] { field.FieldType };
            object[] process_args = new object[] { process_me };

            var method = interface_type.GetMethod("preprocess", process_args_type);
            var processed = method.Invoke(null, process_args);

            field.SetValue(payload, processed);
        }

        return Instance._IJPreProcess(payload);
    }

    public static void delete_children(List<Directories> hierarchie, string name, bool use_install_folder = false, int skip_last_children = 0)
    {
        var new_hierarchie =
            Instance._IJGetHierarchie(hierarchie);
        var new_name =
            Instance._IJGetName(name);

        for (int i = 0; i < JsonSeperateFields.Count() - skip_last_children; i++)
        {
            var field = JsonSeperateFields[i];

            Type interface_type = typeof(IJSONsavable<>).MakeGenericType(field.FieldType);
            Type[] delete_args_type = new Type[] { typeof(List<Directories>), typeof(string), typeof(bool) };
            object[] delete_args = new object[] { new_hierarchie, new_name, use_install_folder };

            var method = interface_type.GetMethod("delete", delete_args_type);
            method.Invoke(null, delete_args);
        }
    }

    public static bool delete(List<Directories> hierarchie, string name, bool use_install_folder = false)
    {
        var new_hierarchie =
            Instance._IJGetHierarchie(hierarchie);
        var new_name =
            Instance._IJGetName(name);

        string path = CreatePathToFile(out bool _, new_name, "JSON", new_hierarchie, use_install_folder);
        if (!delete(path))
            return false;

        delete_children(hierarchie, name, use_install_folder);
        return true;
    }

    // does not delete children!
    private static bool delete(string path)
    {
        if (!File.Exists(path))
            return false;

        File.Delete(path);
        return true;
    }

    // public bool delete() => delete(hierarchie, name);

    #endregion MethodTemplates

}

public static class JSONsavable
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class JsonSeparateAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class JsonAutoPostProcessAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class JsonAutoPreProcessAttribute : Attribute { }

    /// <summary>
    /// Writes the given object instance to a Json file, recursively, including public members, excluding [JsonIgnore].
    /// <para>Object type must have a parameterless constructor.</para>
    /// <para>Only public properties and variables will be written to the file. These can be any type though, even other non-abstract classes.</para>
    /// </summary>
    /// <param name="filePath">The file path to write the object instance to.</param>
    /// <param name="objectToWrite">The object instance to write to the file.</param>
    public static bool WriteToJsonFile(string filePath, object objectToWrite)
    {
        // This tells your serializer that multiple references are okay.
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        TextWriter writer = null;
        try
        {
            string payload = JsonConvert.SerializeObject(objectToWrite, settings);
            writer = new StreamWriter(filePath);
            writer.Write(payload);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }
    }

    /// <summary>
    /// Reads an object instance from an Json file.
    /// <para>Object type must have a parameterless constructor.</para>
    /// </summary>
    /// <typeparam name="T">The type of object to read from the file.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the Json file.</returns>
    public static bool ReadFromJsonFile<T>(out T payload, string filePath) where T : new()
    {
        payload = default(T);
        TextReader reader = null;

        try
        {
            reader = new StreamReader(filePath);
            var fileContents = reader.ReadToEnd();
            payload = JsonConvert.DeserializeObject<T>(fileContents);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
    }

}
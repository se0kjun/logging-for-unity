using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;

public class CommandManager : MonoBehaviour {
    #region SINGLETON
    private static CommandManager _instance;
    public static CommandManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(CommandManager)) as CommandManager;
                if (_instance == null)
                {
                    Debug.LogError("There needs to be one active CommandManager script on a GameObject in your scene.");
                }
            }

            return _instance;
        }
    }
    #endregion

    public List<CommandWrapper> objectList;

    public void AddCommand(GameObject obj, Type type)
    {
    }

    private object SetValueMember(MemberInfo member, object target, object value)
    {
        switch (member.MemberType)
        {
            case MemberTypes.Field:
                ((FieldInfo)member).SetValue(target, value);
                break;
            case MemberTypes.Property:
                ((PropertyInfo)member).SetValue(target, value, null);
                break;
        }

        return target;
    }

    private object GetValueMember(MemberInfo member, object target)
    {
        object tmp = null;
        switch (member.MemberType)
        {
            case MemberTypes.Field:
                tmp = ((FieldInfo)member).GetValue(target);
                break;
            case MemberTypes.Property:
                tmp = ((PropertyInfo)member).GetValue(target, null);
                break;
        }

        return tmp;
    }

    private void SetValueRecursive(GameObject obj, string modifier, object value)
    {
        string[] members = modifier.Split('.');

        object tmp = obj;
        object parent_tmp = obj;
        Stack<object> tmp_list = new Stack<object>();
        tmp_list.Push(obj);

        if (members.Length > 1)
        {
            for (int i = 0; i < members.Length - 1; i++)
            {
                // member metadata
                tmp = tmp.GetType().GetField(members[i]) as MemberInfo
                    ?? tmp.GetType().GetProperty(members[i]) as MemberInfo;

                // value
                tmp = GetValueMember((MemberInfo)tmp, parent_tmp);
                parent_tmp = tmp;
                tmp_list.Push(tmp);
            }
        }

        object set_obj = tmp_list.Pop();
        object set_obj_prev = value;

        for (int i = members.Length - 1; i > 0; i--) 
        {
            MemberInfo realMember = set_obj.GetType().GetField(members[i]) as MemberInfo
                ?? set_obj.GetType().GetProperty(members[i]) as MemberInfo;

            set_obj_prev = SetValueMember(realMember, set_obj, set_obj_prev);
            set_obj = tmp_list.Pop();
        }
    }
}

[Flags]
public enum ControlBuiltInFlags
{
    POSITION,
    ROTATION,
    SCALE,
    OTHER
}

[Serializable]
public class CommandWrapper
{
    public string commandID;

    public GameObject commandObj;
    public ControlBuiltInFlags commandFlags;

    public string commandObjOtherType;
    public string commandObjMember;
}

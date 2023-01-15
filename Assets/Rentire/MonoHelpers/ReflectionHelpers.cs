using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

public static class ReflectionHelpers
{
    public static object GetPropertyValue(this object src, object propName)
    {
        return src.GetType().GetPropertyValue(propName);        
    }
}

public static class ReflectionHelpers<T>
{
    
    public static void CastFieldsIntoList(ref List<T> list, object instance, BindingFlags Flags = BindingFlags.Public, bool includeNonAssigned = false)
    {
        var props = instance.GetType().GetFields(Flags | BindingFlags.Instance);
        for (int i = 0; i < props.Length; i++)
        {
            var prop = props[i];
            if (prop.FieldType == typeof(T) && !list.Contains((T)prop.GetValue(instance)))
            {
                if(!includeNonAssigned && (T)prop.GetValue(instance) == null)
                    continue;
                
                list.Add((T)prop.GetValue(instance));
            }
        }
    }
    
    public static List<string> GetPropertyNames(Type instance)
    {
        var list = new List<string>();
        if (instance == null)
            return new List<string>();

        Type type = instance.GetType(); // MyClass is static class with static properties

        foreach (var p in type.GetProperties())
        {
            list.Add(p.Name);
        }
        return list;
    }

    public static T GetPropertyValueWithName(string name, Type instance)
    {
        if (instance == null)
            return default(T);

        Type type = instance.GetType();

        foreach (var p in type.GetProperties())
        {
            if (p.Name.Equals(name))
            {
                return (T)p.GetValue(null);
            }
        }

        return default(T);
    }

    public static Dictionary<string, string> GetPropertyNamesWithValues(Type instance)
    {
        var list = new Dictionary<string, string>();
        if (instance == null)
            return new Dictionary<string, string>();

        Type type = instance.GetType(); // MyClass is static class with static properties

        foreach (var p in instance.GetProperties())
        {
            list.Add(p.Name, (string)p.GetValue(null, null));
        }
        return list;
    }
    
    public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
        ConstantExpression cExpr = propertyExpression.Body as ConstantExpression;
        MemberExpression mExpr = propertyExpression.Body as MemberExpression;

        if (cExpr != null)
            return cExpr.Value.ToString();
        else if (mExpr != null)
            return mExpr.Member.Name;

        else return null;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class PropertyExtensions
    {
        /// <summary>
        /// var x = new ObjectType();
        // note that in this case we don't need to specify types of x and Property1
        //var propName1 = GetPropertyName(() => x.Property1);
        //// assumes Property2 is an int property
        //var propName2 = GetPropertyName<ObjectType, int>(y => y.Property2);
        //// requires only object type
        //var propName3 = GetPropertyName<ObjectType>(y => y.Property3);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> property)
        {
            PropertyInfo propertyInfo = null;
            var body = property.Body;

            if (body is MemberExpression)
            {
                propertyInfo = (body as MemberExpression).Member as PropertyInfo;
            }
            else if (body is UnaryExpression)
            {
                propertyInfo = ((MemberExpression)((UnaryExpression)body).Operand).Member as PropertyInfo;
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }

            var propertyName = propertyInfo.Name;

            return propertyName;
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            PropertyInfo propertyInfo = null;
            var body = property.Body;

            if (body is MemberExpression)
            {
                propertyInfo = (body as MemberExpression).Member as PropertyInfo;
            }
            else if (body is UnaryExpression)
            {
                propertyInfo = ((MemberExpression)((UnaryExpression)body).Operand).Member as PropertyInfo;
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }

            var propertyName = propertyInfo.Name;

            return propertyName;
        }








    }
}

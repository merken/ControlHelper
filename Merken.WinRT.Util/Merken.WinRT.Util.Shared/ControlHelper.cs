using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Merken.WinRT.Util
{
    /// <summary>
    /// This class allows the caller to search within the XAML visual tree.
    /// The search functionality is based on the VisualTreeHelper from the WinRT framework
    /// </summary>
    public static class ControlHelper
    {
        /// <summary>
        /// Returns the root page of an element
        /// </summary>
        /// <param name="child">The starting point of the search in the visual tree</param>
        /// <returns>The root parent page of an element</returns>
        public static Page GetParentPage(FrameworkElement child)
        {
            var parent = child.Parent;
            var parentPage = (parent as Page);
            while (parentPage == null && parent != null)
            {
                parent = (parent as FrameworkElement).Parent;
                parentPage = (parent as Page);
            }
            return parentPage;
        }

        /// <summary>
        /// Returns the parent of an element
        /// </summary>
        /// <param name="child">The starting point of the search in the visual tree</param>
        /// <returns>The actual parent of the element</returns>
        public static FrameworkElement Parent(DependencyObject child)
        {
            return VisualTreeHelper.GetParent(child) as FrameworkElement;
        }

        /// <summary>
        /// Returns the parent of a specific type
        /// </summary>
        /// <typeparam name="T">The type of the parent to look for, derived from FrameworkElement</typeparam>
        /// <param name="child">The starting point of the search in the visual tree</param>
        /// <param name="filter">Optional, allows the caller to specify the parent that is returned. For example p => p.Name == "LayoutRoot"</param>
        /// <returns>A typed parent</returns>
        public static T FindParent<T>(DependencyObject child, Func<T, bool> filter = null) where T : FrameworkElement
        {
            FrameworkElement parent = VisualTreeHelper.GetParent(child) as FrameworkElement;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null && (filter == null || filter(correctlyTyped)))
                    return correctlyTyped;
                else
                    return FindParent<T>(parent, filter);
            }
            return null;
        }

        /// <summary>
        /// Returns a child from a parent element, based on a specific type
        /// </summary>
        /// <param name="parent">The starting point of the search in the visual tree</param>
        /// <param name="filter">Optional, allows the caller to specify the child that is returned. For example c => c.Name == "MyPanel"</param>
        /// <returns>The first occurrence of a child based on a type</returns>
        public static T Child<T>(FrameworkElement parent, Func<T, bool> filter = null) where T : FrameworkElement
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int c = 0; c < childCount; c++)
            {
                FrameworkElement child = VisualTreeHelper.GetChild(parent, c) as FrameworkElement;
                if (child != null)
                {
                    T correctlyTyped = child as T;
                    if (correctlyTyped != null && (filter == null || filter(correctlyTyped)))
                        return correctlyTyped;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns all the first-level children of an element
        /// </summary>
        /// <param name="parent">The starting point of the search in the visual tree</param>
        /// <returns>A list of FrameworkElements</returns>
        public static IList<FrameworkElement> Children(FrameworkElement parent)
        {
            List<FrameworkElement> children = new List<FrameworkElement>();
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int c = 0; c < childCount; c++)
                children.Add(VisualTreeHelper.GetChild(parent, c) as FrameworkElement);
            return children;
        }

        /// <summary>
        /// Returns all the first-level children of a specific type
        /// </summary>
        /// <typeparam name="T">Type of the children to look for, derived from FrameworkElement/typeparam>
        /// <param name="element">The starting point of the search in the visual tree</param>
        /// <param name="filter">Optional, allows the caller to filter the children that are returned. For example c => c.Visibility == Visibility.Collapsed</param>
        /// <returns>A typed list of first-level child elements</returns>
        public static IList<T> Children<T>(FrameworkElement parent, Func<T, bool> filter = null) where T : FrameworkElement
        {
            List<T> children = new List<T>();
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int c = 0; c < childCount; c++)
            {
                FrameworkElement child = VisualTreeHelper.GetChild(parent, c) as FrameworkElement;
                if (child != null)
                {
                    T correctlyTyped = child as T;
                    if (correctlyTyped != null && (filter == null || filter(correctlyTyped)))
                        children.Add(correctlyTyped);
                }
            }
            return children;
        }

        /// <summary>
        /// Finds the first occurrence of a child based, on a type, using the element as the parent. Searching recursively.
        /// </summary>
        /// <typeparam name="T">Type of the child to look for, derived from FrameworkElement</typeparam>
        /// <param name="parent">The starting point of the search in the visual tree</param>
        /// <param name="filter">Optional, allows the caller to filter the child that is returned. For example c => c.Name == "MyButton"</param>
        /// <returns>A typed child elements</returns>
        public static T FindChild<T>(FrameworkElement parent, Func<T, bool> filter = null) where T : FrameworkElement
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            T found = default(T);
            for (int c = 0; c < childCount; c++)
            {
                FrameworkElement child = VisualTreeHelper.GetChild(parent, c) as FrameworkElement;
                if (child != null && found == default(T))
                {
                    T correctlyTyped = child as T;
                    if (correctlyTyped != null && (filter == null || filter(correctlyTyped)))
                        return correctlyTyped;
                    else
                        found = FindChild<T>(child, filter);
                }
            }
            return found;
        }

        /// <summary>
        /// Finds the all occurrences of a type, using the element as the parent. Searching recursively.
        /// </summary>
        /// <typeparam name="T">Type of the children to look for, derived from FrameworkElement</typeparam>
        /// <param name="parent">The starting point of the search in the visual tree</param>
        /// <param name="filter">Optional, allows the caller to filter the children that are returned. For example c => c.Width >= 150</param>
        /// <returns>A typed list of child elements</returns>
        public static IList<T> FindChildren<T>(FrameworkElement parent, Func<T, bool> filter = null) where T : FrameworkElement
        {
            List<T> children = new List<T>();
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int c = 0; c < childCount; c++)
            {
                FrameworkElement child = VisualTreeHelper.GetChild(parent, c) as FrameworkElement;
                if (child != null)
                {
                    T correctlyTyped = child as T;
                    if (correctlyTyped != null && (filter == null || filter(correctlyTyped)))
                        children.Add(correctlyTyped);
                    else
                        children.AddRange(FindChildren<T>(child, filter));
                }
            }
            return children;
        }

        /// <summary>
        /// Finds the first occurrence of a sibling element, based on a type, using the element's parent as the parent.
        /// </summary>
        /// <typeparam name="T">Type of the sibling to look for, derived from FrameworkElement</typeparam>
        /// <param name="element">The starting point in the visual tree</param>
        /// <param name="filter">Optional, allows the caller to filter the sibling that is returned. For example c => c.Name == "MyButton"</param>
        /// <returns>A typed sibling</returns>
        public static T FindSibling<T>(FrameworkElement element, Func<T, bool> filter = null) where T : FrameworkElement
        {
            T sibling = default(T);
            var parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
            if (parent != null)
                sibling = Child<T>(parent, filter);

            if (sibling != element)
                return sibling;

            return null;
        }

        /// <summary>
        /// Finds the all the sibling elements, based on a type, using the element's parent as the parent.
        /// </summary>
        /// <typeparam name="T">Type of the siblings to look for, derived from FrameworkElement</typeparam>
        /// <param name="element">The starting point in the visual tree</param>
        /// <param name="filter">Optional, allows the caller to filter the sibling that is returned. For example c => c.Visibility == Visibility.Visible</param>
        /// <returns>A typed list of siblings</returns>
        public static IList<T> FindSiblings<T>(FrameworkElement element, Func<T, bool> filter = null) where T : FrameworkElement
        {
            IList<T> siblings = new List<T>();
            var parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
            if (parent != null)
                siblings = Children<T>(parent, filter);

            if (siblings.Any(s => s == element))
                siblings.Remove(siblings.FirstOrDefault(s => s == element));

            return siblings;
        }
    }
}

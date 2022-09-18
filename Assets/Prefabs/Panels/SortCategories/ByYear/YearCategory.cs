/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class YearCategory
{
    public Query query;

    public class Query
    {
        public List<CategoryMember> categorymembers;
    }

    public class CategoryMember
    {
        public string title;

        public string Url { get { return title.Replace(" ", "_"); }}
    }
}

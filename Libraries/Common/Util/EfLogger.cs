using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Frost.Common.Util {

    /// <summary>Logs changes made to EF DbContext</summary>
    public static class EfLogger {

        public static void LogChanges(DbContext context, string logFileName) {
            context.ChangeTracker.DetectChanges(); // Important!

            ObjectContext ctx = ((IObjectContextAdapter) context).ObjectContext;

            List<ObjectStateEntry> objectStateEntryList = ctx.ObjectStateManager
                                                             .GetObjectStateEntries(EntityState.Added | EntityState.Modified | EntityState.Deleted)
                                                             .ToList();

            StreamWriter sw = File.Exists(logFileName)
                                  ? new StreamWriter(logFileName, true, Encoding.UTF8)
                                  : new StreamWriter(File.Create(logFileName));

            if (objectStateEntryList.Count > 0) {
                sw.WriteLine("############################################");
                sw.WriteLine(DateTime.Now);
            }

            foreach (ObjectStateEntry entry in objectStateEntryList.Where(entry => !entry.IsRelationship)) {
                sw.WriteLine("--------------------------------------------");

                switch (entry.State) {
                    case EntityState.Added:
                        sw.Write("Added entity: ");
                        OutputFieldsAndProperties(sw, entry);
                        break;
                    case EntityState.Deleted:
                        sw.Write("Deleted entity: ");
                        OutputFieldsAndProperties(sw, entry);
                        break;
                    case EntityState.Modified:
                        foreach (string propertyName in entry.GetModifiedProperties()) {
                            WriteModifiedProperty(entry, propertyName, sw);
                        }
                        break;
                }
                sw.WriteLine("--------------------------------------------");
            }
            sw.WriteLine("############################################");
            sw.WriteLine();
            sw.Flush();
            sw.Close();
        }

        private static void WriteModifiedProperty(ObjectStateEntry entry, string propertyName, TextWriter sw) {
            DbDataRecord original = entry.OriginalValues;
            string oldValue = original.GetValue(original.GetOrdinal(propertyName)).ToString();

            CurrentValueRecord current = entry.CurrentValues;
            string newValue = current.GetValue(current.GetOrdinal(propertyName)).ToString();

            // probably not necessary 
            if (oldValue == newValue) {
                return;
            }

            if (oldValue == "") {
                oldValue = "<empty string>";
            }
            if (newValue == "") {
                newValue = "<empty string>";
            }
            sw.WriteLine("Entry: {0} Original: {1} New: {2}", entry.Entity.GetType().Name, oldValue, newValue);
        }

        private static void OutputFieldsAndProperties(TextWriter sw, ObjectStateEntry entry) {
            Type t = entry.Entity.GetType();
            Type objectType = ObjectContext.GetObjectType(t);
            if (objectType != null) {
                t = objectType;
            }

            sw.WriteLine(t.Name);

            foreach (MemberInfo member in t.GetMembers().Where(m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property)) {
                sw.WriteLine(member.MemberType + ": " + member.Name);

                object value;
                if (member.MemberType == MemberTypes.Property) {
                    PropertyInfo propertyInfo = (PropertyInfo) member;
                    if (propertyInfo.GetIndexParameters().Length > 0) {
                        continue;
                    }

                    value = propertyInfo.GetValue(entry.Entity);
                }
                else {
                    FieldInfo fieldInfo = (FieldInfo) member;
                    value = fieldInfo.GetValue(entry.Entity);
                }

                sw.WriteLine("Value: " + (value != null ? value : "null"));
                sw.WriteLine();
            }
        }
    }
}

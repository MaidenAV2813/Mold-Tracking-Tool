using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace Tracking_Tool_System.Services
{
    public class ActiveDirectoryHelper
    {
        private readonly string _domain = "icumed.com";

        // 🔐 Authenticate user against Active Directory
        public bool AuthenticateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, _domain))
                {
                    return context.ValidateCredentials(username, password);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 👤 Return SAM Account Name
        public string ReturnUsername(string username)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                return user?.SamAccountName ?? "No User Found";
            }
        }

        // 👥 Return user groups (MODERN List<string>)
        public List<string> ReturnGroups(string username)
        {
            var groupsList = new List<string>();

            using (var context = new PrincipalContext(ContextType.Domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);

                if (user != null)
                {
                    var groups = user.GetGroups();

                    foreach (var group in groups)
                    {
                        if (!string.IsNullOrEmpty(group.Name))
                            groupsList.Add(group.Name);
                    }
                }
            }

            return groupsList;
        }

        // 🧑 Return display name
        public string ReturnName(string username)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                return user?.DisplayName ?? "No User Found";
            }
        }

        // 🔒 Check if user belongs to a group
        public bool IsUserInGroup(string username, string groupName)
        {
            using (var context = new PrincipalContext(ContextType.Domain, _domain))
            {
                var user = UserPrincipal.FindByIdentity(context, IdentityType.UserPrincipalName, username);
                var group = GroupPrincipal.FindByIdentity(context, groupName);

                if (user != null && group != null)
                {
                    return user.IsMemberOf(group);
                }
            }

            return false;
        }

        // 📊 Get all AD attributes
        public Dictionary<string, object> GetAllUserAttributes(string username)
        {
            var userAttributes = new Dictionary<string, object>();

            using (var context = new PrincipalContext(ContextType.Domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);

                if (user != null)
                {
                    var de = user.GetUnderlyingObject() as DirectoryEntry;

                    if (de != null)
                    {
                        using (de)
                        {
                            foreach (string attributeName in de.Properties.PropertyNames)
                            {
                                var propertyCollection = de.Properties[attributeName];

                                if (propertyCollection.Count > 1)
                                {
                                    var values = new List<object>();

                                    foreach (var value in propertyCollection)
                                    {
                                        values.Add(value);
                                    }

                                    userAttributes.Add(attributeName, values);
                                }
                                else
                                {
                                    userAttributes.Add(attributeName, propertyCollection.Value);
                                }
                            }
                        }
                    }

                    return userAttributes;
                }
            }

            return null;
        }

        // 🖼️ Get profile image from AD
        public byte[] GetUserProfileImage(string username)
        {
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    var user = UserPrincipal.FindByIdentity(context, username);

                    if (user != null)
                    {
                        var de = user.GetUnderlyingObject() as DirectoryEntry;

                        if (de != null)
                        {
                            using (de)
                            {
                                if (de.Properties["thumbnailPhoto"].Count > 0)
                                {
                                    return (byte[])de.Properties["thumbnailPhoto"][0];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting profile image: {ex.Message}");
            }

            return null;
        }
    }
}

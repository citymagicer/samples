﻿//<Snippet1>
using System;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;

public class Example
{
    public static void Main()
    {
        // Create a string representing the current user.
        string user = Environment.UserDomainName + "\\" + 
            Environment.UserName;

        // Create a security object that grants no access.
        EventWaitHandleSecurity mSec = new EventWaitHandleSecurity();

        // Add a rule that grants the current user the 
        // right to wait on or signal the event.
        EventWaitHandleAccessRule ruleA = new EventWaitHandleAccessRule(user, 
            EventWaitHandleRights.Synchronize | EventWaitHandleRights.Modify, 
            AccessControlType.Allow);
        mSec.AddAccessRule(ruleA);

        // Add a rule that denies the current user the 
        // right to change permissions on the event.
        EventWaitHandleAccessRule rule = new EventWaitHandleAccessRule(user, 
            EventWaitHandleRights.ChangePermissions, 
            AccessControlType.Deny);
        mSec.AddAccessRule(rule);

        // Display the rules in the security object.
        ShowSecurity(mSec);

        // Add a rule that allows the current user the 
        // right to read permissions on the event. This rule
        // is merged with the existing Allow rule.
        rule = new EventWaitHandleAccessRule(user, 
            EventWaitHandleRights.ReadPermissions, 
            AccessControlType.Allow);
        mSec.AddAccessRule(rule);

        ShowSecurity(mSec);

        // Attempt to remove the original rule (granting
        // the right to wait on or signal the event) with 
        // RemoveAccessRuleSpecific. The removal fails,
        // because the right to read the permissions on the 
        // event has been added to the rule, so that it no 
        // longer matches the original rule.
        Console.WriteLine("Attempt to use RemoveAccessRuleSpecific on the original rule.");
        mSec.RemoveAccessRuleSpecific(ruleA);

        ShowSecurity(mSec);

        // Create a rule that grants the current user 
        // the right to wait on or signal the event, and
        // to read permissions. Use this rule to remove
        // the Allow rule for the current user.
        Console.WriteLine("Use RemoveAccessRuleSpecific with the correct rights.");
        rule = new EventWaitHandleAccessRule(user, 
            EventWaitHandleRights.Synchronize | EventWaitHandleRights.Modify | 
                EventWaitHandleRights.ReadPermissions, 
            AccessControlType.Allow);
        mSec.RemoveAccessRuleSpecific(rule);

        ShowSecurity(mSec);
    }

    private static void ShowSecurity(EventWaitHandleSecurity security)
    {
        Console.WriteLine("\r\nCurrent access rules:\r\n");

        foreach(EventWaitHandleAccessRule ar in 
            security.GetAccessRules(true, true, typeof(NTAccount)))
        {
            Console.WriteLine("        User: {0}", ar.IdentityReference);
            Console.WriteLine("        Type: {0}", ar.AccessControlType);
            Console.WriteLine("      Rights: {0}", ar.EventWaitHandleRights);
            Console.WriteLine();
        }
    }
}

/*This code example produces output similar to following:

Current access rules:

        User: TestDomain\TestUser
        Type: Deny
      Rights: ChangePermissions

        User: TestDomain\TestUser
        Type: Allow
      Rights: Modify, Synchronize


Current access rules:

        User: TestDomain\TestUser
        Type: Deny
      Rights: ChangePermissions

        User: TestDomain\TestUser
        Type: Allow
      Rights: Modify, ReadPermissions, Synchronize

Attempt to use RemoveAccessRuleSpecific on the original rule.

Current access rules:

        User: TestDomain\TestUser
        Type: Deny
      Rights: ChangePermissions

        User: TestDomain\TestUser
        Type: Allow
      Rights: Modify, ReadPermissions, Synchronize

Use RemoveAccessRuleSpecific with the correct rights.

Current access rules:

        User: TestDomain\TestUser
        Type: Deny
      Rights: ChangePermissions
 */
//</Snippet1>

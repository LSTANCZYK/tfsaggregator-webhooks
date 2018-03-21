﻿namespace Aggregator.WebHooks.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Aggregator.Core.Interfaces;
    using Newtonsoft.Json.Linq;

    internal class WorkItemRequest
    {
        // pseudo-data
        internal bool IsValid
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Error);
            }
        }

        internal string Error { get; private set; }

        // real data
        internal string EventId { get; private set; }

        internal string EventType { get; private set; }

        // EventType property decoded
        internal ChangeTypes ChangeType { get; private set; }

        internal string AccountId { get; private set; }

        public string CollectionId { get; private set; }

        internal int WorkItemId { get; private set; }

        internal string TeamProject { get; private set; }

        internal string TfsCollectionUri { get; private set; }

        private WorkItemRequest()
        {
            this.Error = string.Empty;
        }

        internal static WorkItemRequest Parse(JObject payload)
        {
            var result = new WorkItemRequest();

            try
            {
                if (payload.Property("eventType") == null)
                {
                    result.Error = $"Could not determine event type for message: {payload}";
                }
                else
                {
                    result.EventType = (string)payload["eventType"];
                    result.EventId = (string)payload["id"];

                    // TODO in the future we will use also the Organization level
                    if (payload.Property("resourceContainers") == null)
                    {
                        // bloody Test button
                        result.Error = $"Test button generates bad messages: do not use with this service.";
                    }
                    else
                    {
                        // VSTS sprint 100 or so introduced the Account, but TFS 2015.3 stil lacks it
                        if (payload.SelectToken("resourceContainers.account") == null)
                        {
                            // TFS
                            result.CollectionId = (string)payload["resourceContainers"]["collection"]["id"];
                        }
                        else
                        {
                            result.AccountId = (string)payload["resourceContainers"]["account"]["id"];
                        }
                    }

                    string fullUrl = (string)payload["resource"]["url"];
                    result.TfsCollectionUri = fullUrl.Substring(0, fullUrl.IndexOf("_apis"));

                    /* TODO
                    ["resource"]["fields"]["System.TeamProject"]
                    rules out using 'Minimal' for 'Resource detail to send'
                    search a solution!!!
                    */
                    switch (result.EventType)
                    {
                        case "workitem.created":
                            result.WorkItemId = (int)payload["resource"]["id"];
                            result.TeamProject = (string)payload["resource"]["fields"]["System.TeamProject"];
                            result.ChangeType = Core.Interfaces.ChangeTypes.New;
                            break;
                        case "workitem.updated":
                            result.WorkItemId = (int)payload["resource"]["workItemId"];
                            result.TeamProject = (string)payload["resource"]["revision"]["fields"]["System.TeamProject"];
                            result.ChangeType = Core.Interfaces.ChangeTypes.Change;
                            break;
                        case "workitem.restored":
                            result.WorkItemId = (int)payload["resource"]["id"];
                            result.TeamProject = (string)payload["resource"]["fields"]["System.TeamProject"];
                            result.ChangeType = Core.Interfaces.ChangeTypes.Restore;
                            break;
                        case "workitem.deleted":
                            result.WorkItemId = (int)payload["resource"]["id"];
                            result.TeamProject = (string)payload["resource"]["fields"]["System.TeamProject"];
                            result.ChangeType = Core.Interfaces.ChangeTypes.Delete;
                            break;
                        default:
                            result.Error = $"Unsupported eventType {result.EventType}";
                            break;
                    }//switch
                }//if
            }
            catch (Exception e)
            {
                result.Error = $"Failed to parse incoming notification from TFS/VSTS. {e.Message}";
            }

            return result;
        }
    }
}
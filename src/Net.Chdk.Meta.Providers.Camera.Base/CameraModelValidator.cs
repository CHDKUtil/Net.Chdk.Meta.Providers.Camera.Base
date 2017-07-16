﻿using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers
{
    public abstract class CameraModelValidator : ICameraModelValidator
    {
        protected ILogger Logger { get; }

        protected CameraModelValidator(ILogger logger)
        {
            Logger = logger;
        }

        public void Validate(string platform, ListPlatformData list, TreePlatformData tree)
        {
            foreach (var kvp in tree.Revisions)
                Validate(kvp, platform, list);

            foreach (var kvp in list.Revisions)
                Validate(kvp, platform, tree);
        }

        private void Validate(KeyValuePair<string, TreeRevisionData> kvp, string platform, ListPlatformData list)
        {
            var revision = kvp.Key;
            if (!list.Revisions.ContainsKey(revision))
                OnListRevisionMissing(platform, revision);
        }

        private void Validate(KeyValuePair<string, ListRevisionData> kvp, string platform, TreePlatformData tree)
        {
            var revision = kvp.Value.Source?.Revision ?? kvp.Key;
            if (!tree.Revisions.ContainsKey(revision))
                OnTreeRevisionMissing(platform, kvp.Key);
        }

        protected virtual void OnListRevisionMissing(string platform, string revision)
        {
            throw new InvalidOperationException($"{platform}: {revision} missing from list");
        }

        protected virtual void OnTreeRevisionMissing(string platform, string revision)
        {
            throw new InvalidOperationException($"{platform}: {revision} missing from tree");
        }
    }
}

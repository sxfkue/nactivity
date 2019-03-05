﻿using System;
using System.Collections.Generic;

/* Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace org.activiti.engine.impl.persistence.entity
{
    using org.activiti.bpmn.constants;
    using org.activiti.engine.impl.context;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// 
    /// 
    [Serializable]
    public class DeploymentEntityImpl : AbstractEntityNoRevision, IDeploymentEntity
    {

        private const long serialVersionUID = 1L;

        protected internal string name;
        protected internal string category;
        protected internal string key;
        protected internal string tenantId = ProcessEngineConfiguration.NO_TENANT_ID;
        protected internal IDictionary<string, IResourceEntity> resources;
        protected internal DateTime deploymentTime;
        protected internal bool isNew;

        // Backwards compatibility
        protected internal string engineVersion;

        /// <summary>
        /// Will only be used during actual deployment to pass deployed artifacts (eg process definitions). Will be null otherwise.
        /// </summary>
        protected internal IDictionary<Type, IList<object>> deployedArtifacts;

        public DeploymentEntityImpl()
        {

        }

        public virtual void addResource(IResourceEntity resource)
        {
            if (resources == null)
            {
                resources = new Dictionary<string, IResourceEntity>();
            }
            resources[resource.Name] = resource;
        }

        // lazy loading ///////////////////////////////////////////////////////////////

        public virtual IDictionary<string, IResourceEntity> GetResources()
        {
            var ctx = Context.CommandContext;
            if (resources == null && id != null && ctx != null)
            {
                IList<IResourceEntity> resourcesList = ctx.ResourceEntityManager.findResourcesByDeploymentId(id);
                resources = new Dictionary<string, IResourceEntity>();
                foreach (IResourceEntity resource in resourcesList)
                {
                    resources[resource.Name] = resource;
                }
            }
            return resources;
        }

        public virtual void SetResources(IDictionary<string, IResourceEntity> value)
        {
            this.resources = value;
        }

        public override PersistentState PersistentState
        {
            get
            {
                PersistentState persistentState = new PersistentState();

                persistentState["category"] = this.category;
                persistentState["key"] = this.key;
                persistentState["tenantId"] = tenantId;

                return persistentState;
            }
        }

        // Deployed artifacts manipulation ////////////////////////////////////////////

        public virtual void addDeployedArtifact(object deployedArtifact)
        {
            if (deployedArtifacts == null)
            {
                deployedArtifacts = new Dictionary<Type, IList<object>>();
            }

            Type clazz = deployedArtifact.GetType();
            deployedArtifacts.TryGetValue(clazz, out IList<object> artifacts);
            if (artifacts == null)
            {
                artifacts = new List<object>();
                deployedArtifacts[clazz] = artifacts;
            }

            artifacts.Add(deployedArtifact);
        }

        public virtual IList<T> getDeployedArtifacts<T>()
        {
            Type clazz = typeof(T);

            List<T> list = new List<T>();
            foreach (Type deployedArtifactsClass in deployedArtifacts.Keys)
            {
                if (clazz.IsAssignableFrom(deployedArtifactsClass))
                {
                    list.AddRange(deployedArtifacts[deployedArtifactsClass].Cast<T>().ToList());
                }
            }

            return list;
        }

        public virtual void unrunable()
        {
            foreach (string key in (resources ?? new Dictionary<string, IResourceEntity>()).Keys)
            {
                IResourceEntity resource = resources[key];
                if (resource.Bytes?.Length == 0)
                {
                    continue;
                }

                runable(resource, false);
            }
        }

        public virtual void runable()
        {
            foreach (string key in (resources ?? new Dictionary<string, IResourceEntity>()).Keys)
            {
                IResourceEntity resource = resources[key];
                if (resource.Bytes?.Length == 0)
                {
                    continue;
                }

                runable(resource, true);
            }
        }

        private static void runable(IResourceEntity resource, bool runable)
        {
            using (MemoryStream ms = new MemoryStream(resource.Bytes))
            {
                XDocument doc = XDocument.Load(ms);
                XElement process = doc.Descendants(XName.Get("process", BpmnXMLConstants.BPMN2_NAMESPACE)).First();
                process.Attribute("isExecutable").SetValue(runable);
                resource.Bytes = Encoding.UTF8.GetBytes(doc.ToString());
            }
        }

        // getters and setters ////////////////////////////////////////////////////////

        public virtual string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }

        public virtual string BusinessKey { get; set; }

        public virtual string BusinessPath { get; set; }

        public string StartForm { get; set; }


        public virtual string Category
        {
            get
            {
                return category;
            }
            set
            {
                this.category = value;
            }
        }


        public virtual string Key
        {
            get
            {
                return key;
            }
            set
            {
                this.key = value;
            }
        }


        public virtual string TenantId
        {
            get
            {
                return tenantId;
            }
            set
            {
                this.tenantId = value;
            }
        }



        public virtual DateTime DeploymentTime
        {
            get
            {
                return deploymentTime;
            }
            set
            {
                this.deploymentTime = value;
            }
        }


        public virtual bool New
        {
            get
            {
                return isNew;
            }
            set
            {
                this.isNew = value;
            }
        }


        public virtual string EngineVersion
        {
            get
            {
                return engineVersion;
            }
            set
            {
                this.engineVersion = value;
            }
        }


        // common methods //////////////////////////////////////////////////////////

        public override string ToString()
        {
            return "DeploymentEntity[id=" + id + ", name=" + name + "]";
        }

    }
}
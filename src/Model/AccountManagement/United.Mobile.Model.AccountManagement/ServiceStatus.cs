// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceStatus.cs" company="United Continental Holdings, Inc.">
//   Copyright (c) 2013 United Continental Holdings, Inc. All rights reserved.
// </copyright>
// <summary>
//   $Id: ServiceStatus.cs 129 2012-10-18 20:15:50Z bob.wang $
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
namespace United.Mobile.Model.Common
{
    /// <summary>
    /// The service exception.
    /// </summary>
    [Serializable()]
    public class ServiceStatus
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public virtual string Message { get; set; }
    }
}

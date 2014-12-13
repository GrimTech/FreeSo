﻿/*The contents of this file are subject to the Mozilla Public License Version 1.1
(the "License"); you may not use this file except in compliance with the
License. You may obtain a copy of the License at http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS" basis,
WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
the specific language governing rights and limitations under the License.

The Original Code is the GonzoNet.

The Initial Developer of the Original Code is
Mats 'Afr0' Vederhus. All Rights Reserved.

Contributor(s): ______________________________________.
*/

using System;
using System.Collections.Generic;
using System.Text;
using GonzoNet.Events;

namespace GonzoNet.Exceptions
{
    public class DecryptionException : Exception
    {
        public EventCodes ErrorCode = EventCodes.PACKET_DECRYPTION_ERROR;

        public DecryptionException(string Description)
            : base(Description)
        {

        }
    }
}

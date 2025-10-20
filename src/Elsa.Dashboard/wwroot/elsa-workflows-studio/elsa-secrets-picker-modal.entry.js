import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';
import './credential-manager-plugin-CzGEoBmR.js';
import { s as state } from './secret.store-Bx5ZyYIY.js';
import { S as SecretEventTypes } from './secret.events-CkUpytGo.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import './index-D7wXd6HU.js';
import './events-CpKc8CLe.js';
import './index-fZDMH_YE.js';
import './elsa-client-q6ob5JPZ.js';
import './fetch-client-1OcjQcrw.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';
import './dashboard-DaK-DIvX.js';
import './index-C-8L13GY.js';
import './store-B_H_ZDGs.js';

const SecretIcon = props => {
    const color = props.color || 'sky';
    return (`<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${color}-50 elsa-text-${color}-700 elsa-ring-4 elsa-ring-white`}">
      <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" aria-hidden="true">
        <path stroke="none" d="M0 0h24v24H0z"/>  <polyline points="21 12 17 12 14 20 10 4 7 12 3 12"/>
      </svg>
    </span>`);
};

const Ampq = {
    category: "AMPQ",
    customAttributes: {},
    description: "AMPQ connection string",
    displayName: "AMPQ",
    inputProperties: [
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Hostname",
            name: "hostname",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Port",
            name: "port",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.Int64",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "User",
            name: "user",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Password",
            name: "password",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Transport Type",
            name: "transport_type",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        }
    ],
    type: "AMPQ"
};

const PostgreSql = {
    category: "Sql",
    customAttributes: {},
    description: "Sql connection string",
    displayName: "PostgreSql",
    inputProperties: [
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Host",
            name: "Host",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Port",
            name: "Port",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.Int64",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Database",
            name: "Database",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Username",
            name: "Username",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Password",
            name: "Password",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Additional Settings",
            name: "AdditionalSettings",
            hint: "The content entered will be appended to the end of the generated connection string.",
            order: 20,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
    ],
    type: "PostgreSql"
};

const SqlServer = {
    category: "Sql",
    customAttributes: {},
    description: "Sql connection string",
    displayName: "MSSQL Server",
    inputProperties: [
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Data Source",
            name: "Data Source",
            hint: "FQDN/IP to Server",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Initial Catalog",
            name: "Initial Catalog",
            hint: "Database name",
            order: 1,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.Int64",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "User ID",
            name: "User ID",
            hint: "Username for connection",
            order: 2,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.Int64",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Password",
            name: "Password",
            hint: "Password for connection",
            order: 3,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Connection Timeout",
            name: "Connection Timeout",
            hint: "The length of time (in seconds) to wait for a connection to the server before terminating the attempt and generating an error.",
            order: 5,
            defaultValue: 15,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.Int64",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Integrated Security",
            name: "Integrated Security",
            hint: "When false, User ID and Password are specified in the connection. When true, the current Windows account credentials are used for authentication. (Default: False)",
            order: 6,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: '',
                        value: null,
                    },
                    {
                        text: 'True',
                        value: 'True',
                    },
                    {
                        text: 'False',
                        value: 'False',
                    }
                ],
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Persist Security Info",
            name: "Persist Security Info",
            hint: "When set to False (strongly recommended), security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state. (Default: False)",
            order: 7,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: '',
                        value: null,
                    },
                    {
                        text: 'True',
                        value: 'True',
                    },
                    {
                        text: 'False',
                        value: 'False',
                    }
                ],
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Pooling",
            name: "Pooling",
            hint: "When the value of this key is set to true, any newly created connection will be added to the pool when closed by the application. In a next attempt to open the same connection, that connection will be drawn from the pool. (Default: True)",
            order: 8,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: '',
                        value: null,
                    },
                    {
                        text: 'True',
                        value: 'True',
                    },
                    {
                        text: 'False',
                        value: 'False',
                    }
                ],
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Trust Server Certificate",
            name: "TrustServerCertificate",
            hint: "When set to true, SSL is used to encrypt the channel when bypassing walking the certificate chain to validate trust. If Trust Server Certificate is set to true and Encrypt is set to false, the channel is not encrypted. (Default: False)",
            order: 9,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: '',
                        value: null,
                    },
                    {
                        text: 'True',
                        value: 'True',
                    },
                    {
                        text: 'False',
                        value: 'False',
                    }
                ],
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Encrypt",
            name: "Encrypt",
            hint: "When true, SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed. (Default: False)",
            order: 10,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: '',
                        value: null,
                    },
                    {
                        text: 'True',
                        value: 'True',
                    },
                    {
                        text: 'False',
                        value: 'False',
                    }
                ],
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Multiple Active Result Sets",
            name: "MultipleActiveResultSets",
            hint: "When true, an application can maintain multiple active result sets. When false, an application must process or cancel all result sets from one batch before it can execute any other batch on that connection. (Default: False)",
            order: 11,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: '',
                        value: null,
                    },
                    {
                        text: 'True',
                        value: 'True',
                    },
                    {
                        text: 'False',
                        value: 'False',
                    }
                ],
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Application Name",
            name: "Application Name",
            hint: "The name of the application",
            order: 12,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Application Intent",
            name: "ApplicationIntent",
            hint: "Declares the application workload type when connecting to a server. (Default: ReadWrite)",
            order: 13,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: '',
                        value: null,
                    },
                    {
                        text: 'Read/Write',
                        value: 'ReadWrite',
                    },
                    {
                        text: 'Read Only',
                        value: 'ReadOnly',
                    }
                ],
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Asynchronous Processing",
            name: "Async",
            hint: "Enables asynchronous operation support. (Default: False)",
            order: 14,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: '',
                        value: null,
                    },
                    {
                        text: 'True',
                        value: 'True',
                    },
                    {
                        text: 'False',
                        value: 'False',
                    }
                ],
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Load Balance Timeout",
            name: "Load Balance Timeout",
            hint: "When a connection is returned to the pool, its creation time is compared with the current time, and the connection is destroyed if that time span (in seconds) exceeds the value specified by Load Balance Timeout. (Default: 0)",
            order: 5,
            defaultValue: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.Int64",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Connect Retry Count",
            name: "ConnectRetryCount",
            hint: "Controls the number of reconnection attempts after the client identifies an idle connection failure. (Default: 1)",
            order: 5,
            defaultValue: 1,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.Int64",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Connect Retry Interval",
            name: "ConnectRetryInterval",
            hint: "Specifies the time between each connection retry attempt. Valid values are 1 to 60 seconds (Default: 10).",
            order: 5,
            defaultValue: 10,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.Int64",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Multi Subnet Failover",
            name: "MultiSubnetFailover",
            hint: "Multi Subnet Failover configures SqlClient to provide faster detection of and connection to the (currently) active server. (Default: False)",
            order: 14,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: '',
                        value: null,
                    },
                    {
                        text: 'True',
                        value: 'True',
                    },
                    {
                        text: 'False',
                        value: 'False',
                    }
                ],
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Additional Settings",
            name: "AdditionalSettings",
            hint: "The content entered will be appended to the end of the generated connection string.",
            order: 20,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
    ],
    type: "MSSQLServer"
};

const MySqlServer = {
    category: "Sql",
    customAttributes: {},
    description: "Sql connection string",
    displayName: "MySQL Server",
    inputProperties: [
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Server",
            name: "server",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Port",
            name: "port",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.Int64",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Database",
            name: "database",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "User ID",
            name: "user id",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Password",
            name: "password",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Allow Load Local Infile",
            name: "AllowLoadLocalInfile",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "SSL Mode",
            name: "SSL Mode",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "SSL Mode",
            name: "SSL Mode",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Certificate File",
            name: "CertificateFile",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Certificate Password",
            name: "CertificatePassword",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Use Affected Rows",
            name: "UseAffectedRows",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Use Compression",
            name: "UseCompression",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Pooling",
            name: "Pooling",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Integrated Security",
            name: "IntegratedSecurity",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Connection Timeout",
            name: "Connection Timeout",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.Int64",
            uiHint: "single-line",
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Additional Settings",
            name: "AdditionalSettings",
            hint: "The content entered will be appended to the end of the generated connection string.",
            order: 20,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        },
    ],
    type: "MySQLServer"
};

const Token = {
    category: "Http",
    customAttributes: {},
    description: "Authorization token",
    displayName: "Authorization",
    inputProperties: [
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: "Authorization",
            name: "Authorization",
            order: 0,
            supportedSyntaxes: ["JavaScript", "Liquid"],
            type: "System.String",
            uiHint: "single-line",
        }
    ],
    type: "Authorization"
};

const OAuth2 = {
    category: 'Http',
    customAttributes: {},
    description: 'OAuth2 credentials',
    displayName: 'OAuth2 credentials',
    inputProperties: [
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: 'Grant Type',
            name: 'GrantType',
            order: 0,
            supportedSyntaxes: ['Literal'],
            type: 'System.String',
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: 'Client Credentials',
                        value: 'client_credentials',
                    },
                    {
                        text: 'Authorization Code',
                        value: 'authorization_code',
                    }
                ],
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: 'Authorization URL',
            name: 'AuthorizationUrl',
            order: 0,
            supportedSyntaxes: ['Literal'],
            type: 'System.String',
            uiHint: 'single-line',
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: 'Access Token URL',
            name: 'AccessTokenUrl',
            order: 1,
            supportedSyntaxes: ['Literal'],
            type: 'System.String',
            uiHint: 'single-line',
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: 'Client ID',
            name: 'ClientId',
            order: 2,
            supportedSyntaxes: ['Literal'],
            type: 'System.String',
            uiHint: 'single-line',
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: 'Client Secret',
            name: 'ClientSecret',
            order: 3,
            supportedSyntaxes: ['Literal'],
            type: 'System.String',
            uiHint: 'single-line',
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: 'Token endpoint client authentication method',
            name: 'ClientAuthMethod',
            order: 4,
            supportedSyntaxes: ['Literal'],
            type: 'System.String',
            uiHint: 'dropdown',
            options: {
                isFlagsEnum: false,
                items: [
                    {
                        text: 'Client secret: Basic',
                        value: 'client_secret_basic',
                    },
                    {
                        text: 'Client secret: Post',
                        value: 'client_secret_post',
                    }
                ]
            }
        },
        {
            disableWorkflowProviderSelection: false,
            isBrowsable: true,
            isReadOnly: false,
            label: 'Scope',
            name: 'Scope',
            order: 5,
            supportedSyntaxes: ['Literal'],
            type: 'System.String',
            uiHint: 'single-line',
        }
    ],
    type: 'OAuth2',
};

const ElsaSecretsPickerModal = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.selectedTrait = 7;
        this.selectedCategory = 'All';
        this.categories = [];
        this.filteredSecretsDescriptorDisplayContexts = [];
        this.onShowSecretsPicker = async () => {
            await this.dialog.show(true);
        };
    }
    connectedCallback() {
        eventBus.on(SecretEventTypes.ShowSecretsPicker, this.onShowSecretsPicker);
    }
    disconnectedCallback() {
        eventBus.detach(SecretEventTypes.ShowSecretsPicker, this.onShowSecretsPicker);
    }
    componentWillRender() {
        const secretsDescriptors = [Ampq, PostgreSql, SqlServer, MySqlServer, Token, OAuth2];
        state.secretsDescriptors = secretsDescriptors;
        this.categories = ['All', ...secretsDescriptors.map(x => x.category).distinct().sort()];
        const searchText = this.searchText ? this.searchText.toLowerCase() : '';
        let filteredSecretsDescriptors = secretsDescriptors;
        if (searchText.length > 0) {
            filteredSecretsDescriptors = filteredSecretsDescriptors.filter(x => {
                const category = x.category || '';
                const description = x.description || '';
                const displayName = x.displayName || '';
                const type = x.type || '';
                return category.toLowerCase().indexOf(searchText) >= 0
                    || description.toLowerCase().indexOf(searchText) >= 0
                    || displayName.toLowerCase().indexOf(searchText) >= 0
                    || type.toLowerCase().indexOf(searchText) >= 0;
            });
        }
        else {
            filteredSecretsDescriptors = !this.selectedCategory || this.selectedCategory == 'All' ? filteredSecretsDescriptors : filteredSecretsDescriptors.filter(x => x.category == this.selectedCategory);
        }
        this.filteredSecretsDescriptorDisplayContexts = filteredSecretsDescriptors.map(x => {
            return {
                secretDescriptor: x,
                secretIcon: h(SecretIcon, { color: 'rose' })
            };
        });
    }
    selectTrait(trait) {
        this.selectedTrait = trait;
    }
    selectCategory(category) {
        this.selectedCategory = category;
    }
    onTraitClick(e, trait) {
        e.preventDefault();
        this.selectTrait(trait);
    }
    onCategoryClick(e, category) {
        e.preventDefault();
        this.selectCategory(category);
    }
    onSearchTextChange(e) {
        this.searchText = e.target.value;
    }
    async onCancelClick() {
        await this.dialog.hide(true);
    }
    async onSecretClick(e, secretDescriptor) {
        e.preventDefault();
        eventBus.emit(SecretEventTypes.SecretPicked, this, secretDescriptor);
        await this.dialog.hide(false);
    }
    render() {
        const selectedCategoryClass = 'elsa-bg-gray-100 elsa-text-gray-900 elsa-flex';
        const defaultCategoryClass = 'elsa-text-gray-600 hover:elsa-bg-gray-50 hover:elsa-text-gray-900';
        const filteredDisplayContexts = this.filteredSecretsDescriptorDisplayContexts;
        const categories = this.categories;
        return (h(Host, { key: '283b36c0bcc67fe550f520c8184deb90fddf2620', class: "elsa-block" }, h("elsa-modal-dialog", { key: '4cb0f48b232b12c73b35916dd1745ebaeb973e60', ref: el => this.dialog = el }, h("div", { key: '9b772c136c241f7d9274538f2382de1ecc3788ec', slot: "content", class: "elsa-py-8" }, h("div", { key: '5ee56dc628c7693dc9c218fe6771b44d70f0dcac', class: "elsa-flex" }, h("div", { key: 'a61122ea1d5b77dac85aa834cbf392d67663fd63', class: "elsa-px-8" }, h("nav", { key: 'fbbd60fb399ac63dd04948cf5d4161f7e6dc4970', class: "elsa-space-y-1", "aria-label": "Sidebar" }, categories.map(category => (h("a", { href: "#", onClick: e => this.onCategoryClick(e, category), class: `${category == this.selectedCategory ? selectedCategoryClass : defaultCategoryClass} elsa-text-gray-600 hover:elsa-bg-gray-50 hover:elsa-text-gray-900 elsa-flex elsa-items-center elsa-px-3 elsa-py-2 elsa-text-sm elsa-font-medium elsa-rounded-md` }, h("span", { class: "elsa-truncate" }, category)))))), h("div", { key: '716826cf31bd7ddcba185e62ffdf5a8f3e0068aa', class: "elsa-flex-1 elsa-pr-8" }, h("div", { key: 'd7e3a43a7ef2241ea84e08cb6a7e1a5305e6971f', class: "elsa-p-0 elsa-mb-6" }, h("div", { key: '615ff95a05cd22c6363f9c5845ed64af0fec1805', class: "elsa-relative elsa-rounded-md elsa-shadow-sm" }, h("div", { key: '6e59d36290f0282c2b8fc58414844eb732ffd689', class: "elsa-absolute elsa-inset-y-0 elsa-left-0 elsa-pl-3 elsa-flex elsa-items-center elsa-pointer-events-none" }, h("svg", { key: '04ab0eaf20a4ead654253f01d4ad970c5b29dae8', class: "elsa-h-6 elsa-w-6 elsa-text-gray-400", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { key: '3607c742e8e914326806e859080a5588dad325f8', stroke: "none", d: "M0 0h24v24H0z" }), h("circle", { key: '9ad25c1ea70dbe3b3e416743542edbe6810dd660', cx: "10", cy: "10", r: "7" }), h("line", { key: '863ab7bf360de314318263aa1a76e930bc5c4567', x1: "21", y1: "21", x2: "15", y2: "15" }))), h("input", { key: '8f8177331a7b83bdf3eccfc436ff94a3509bc89c', type: "text", value: this.searchText, onInput: e => this.onSearchTextChange(e), class: "form-input elsa-block elsa-w-full elsa-pl-10 sm:elsa-text-sm sm:elsa-leading-5 focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-rounded-md elsa-border-gray-300", placeholder: "Search secrets" }))), h("div", { key: 'c5d3b2a1391d684f4472f41c12a55571bb1fc569', class: "elsa-max-w-4xl elsa-mx-auto elsa-p-0" }, categories.map(category => {
            const displayContexts = filteredDisplayContexts.filter(x => x.secretDescriptor.category == category);
            if (displayContexts.length == 0)
                return undefined;
            return (h("div", null, h("h2", { class: "elsa-my-4 elsa-text-lg elsa-leading-6 elsa-font-medium" }, category), h("div", { class: "elsa-divide-y elsa-divide-gray-200 sm:elsa-divide-y-0 sm:elsa-grid sm:elsa-grid-cols-2 sm:elsa-gap-px" }, displayContexts.map(displayContext => (h("a", { href: "#", onClick: e => this.onSecretClick(e, displayContext.secretDescriptor), class: "elsa-relative elsa-rounded elsa-group elsa-p-6 focus-within:elsa-ring-2 focus-within:elsa-ring-inset focus-within:elsa-ring-blue-500" }, h("div", { class: "elsa-flex elsa-space-x-10" }, h("div", { class: "elsa-flex elsa-flex-0 elsa-items-center" }, h("div", { innerHTML: displayContext.secretIcon })), h("div", { class: "elsa-flex-1 elsa-mt-2" }, h("h3", { class: "elsa-text-lg elsa-font-medium" }, h("a", { href: "#", class: "focus:elsa-outline-none" }, h("span", { class: "elsa-absolute elsa-inset-0", "aria-hidden": "true" }), displayContext.secretDescriptor.displayName)), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, displayContext.secretDescriptor.description)))))))));
        }))))), h("div", { key: 'fa9825c74f425bcd8a3b99ae56790b647a0b24ed', slot: "buttons" }, h("div", { key: '29c92730bfba3a9197abce04d71b2230ce0e81be', class: "elsa-bg-gray-50 elsa-px-4 elsa-py-3 sm:elsa-px-6 sm:elsa-flex sm:elsa-flex-row-reverse" }, h("button", { key: '7d0e25c0272d9aa76e7b3f1a164b610f4cb398d0', type: "button", onClick: () => this.onCancelClick(), class: "elsa-mt-3 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-mt-0 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, "Cancel"))))));
    }
};

export { ElsaSecretsPickerModal as elsa_secrets_picker_modal };
//# sourceMappingURL=elsa-secrets-picker-modal.entry.esm.js.map

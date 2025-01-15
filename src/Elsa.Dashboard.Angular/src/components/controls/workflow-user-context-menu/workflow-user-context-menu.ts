import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { AuthenticationConfguration, ElsaClientService, UserDetail } from '../../../services/elsa-client';
import { DropdownButtonItem, DropdownButtonOrigin } from '../workflow-dropdown-button/models';
import { CommonModule } from '@angular/common';
import { WorkflowDropdownButton } from '../workflow-dropdown-button/workflow-dropdown-button';

@Component({
  selector: 'workflow-user-context-menu',
  templateUrl: './workflow-user-context-menu.html',
  styleUrls: ['./workflow-user-context-menu.css'],
  imports: [WorkflowDropdownButton, CommonModule]
})
export class WorkflowUserContextMenu  {
  @Input() serverUrl: string;
  userDetail: UserDetail = null;
  authenticationConfguration: AuthenticationConfguration;
  origin: DropdownButtonOrigin = DropdownButtonOrigin.TopRight;

  ddlitems: Array<DropdownButtonItem> = [{ 'text': ("logout"), value: "logout" }].map(x => {
    const item: DropdownButtonItem = { text: x.text, isSelected: false, value: x.value };
    return item
  });

  logoutStrategy = {
    "OpenIdConnect": function () {
      window.location.href = 'v1/ElsaAuthentication/logout';
    },
    "ServerManagedCookie": function () {
      window.location.href = 'v1/ElsaAuthentication/logout';
    },
    "JwtBearerToken": ""
  };

  constructor(private elsaClientService: ElsaClientService, private http: HttpClient) {
    this.userDetail = {
      name: "Lorna Birchall",
      tenantId: "XXX",
      isAuthenticated: true,
    }
  }

  async componentWillRender() {
    try {
      this.userDetail = await (await (await this.elsaClientService.createElsaClient(this.serverUrl)).authenticationApi.getUserDetails());
      this.authenticationConfguration = await (await (await this.elsaClientService.createElsaClient(this.serverUrl)).authenticationApi.getAuthenticationConfguration());
    } catch (err) {
      this.userDetail = null;
    }
  }

  public async menuItemSelected(item: DropdownButtonItem) {
    if (item.value == 'logout') {
      this.authenticationConfguration.authenticationStyles.forEach(x => {
        this.logoutStrategy[x]();
      });
    }
  }

}





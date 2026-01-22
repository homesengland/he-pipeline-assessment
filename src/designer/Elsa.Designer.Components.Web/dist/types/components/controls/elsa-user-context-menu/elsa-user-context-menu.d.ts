import { DropdownButtonItem } from '../elsa-dropdown-button/models';
import { AuthenticationConfguration, UserDetail } from "../../../services/elsa-client";
export declare class ElsaUserContextMenu {
  serverUrl: string;
  userDetail: UserDetail;
  authenticationConfguration: AuthenticationConfguration;
  componentWillRender(): Promise<void>;
  logoutStrategy: {
    OpenIdConnect: () => void;
    ServerManagedCookie: () => void;
    JwtBearerToken: string;
  };
  menuItemSelected(item: DropdownButtonItem): Promise<void>;
  render(): any;
}

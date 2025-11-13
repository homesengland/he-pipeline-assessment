import {Component, h, Prop, State, getAssetPath} from '@stencil/core';
import {loadTranslations} from "../../../i18n/i18n-loader";
import {resources} from "./localizations";
import {i18n, t} from "i18next";
import {GetIntlMessage} from "../../../i18n/intl-message";
import Tunnel from "../../../../data/dashboard";
import {EventTypes, ConfigureDashboardMenuContext} from '../../../../models';
import {eventBus} from '../../../../services';
import { DropdownButtonItem, DropdownButtonOrigin } from '../../../controls/elsa-dropdown-button/models';

@Component({
  tag: 'elsa-studio-dashboard',
  shadow: false,
  assetsDirs: ['assets']
})
export class ElsaStudioDashboard {

  @Prop({attribute: 'culture', reflect: true}) culture: string;
  @Prop({attribute: 'base-path', reflect: true}) basePath: string = '';
  @State() currentPath: string = window.location.pathname;
  private i18next: i18n;
  private dashboardMenu: ConfigureDashboardMenuContext  = {
    data: {
      menuItems: [
        ['workflow-definitions', 'Workflow Definitions'],
        ['workflow-instances', 'Workflow Instances'],
        ['workflow-registry', 'Workflow Registry'],
      ],
      routes: [
        ['', 'elsa-studio-home', true],
        ['workflow-registry', 'elsa-studio-workflow-registry', true],
        ['workflow-registry/:id', 'elsa-studio-workflow-blueprint-view'],
        ['workflow-definitions', 'elsa-studio-workflow-definitions-list', true],
        ['workflow-definitions/:id', 'elsa-studio-workflow-definitions-edit'],
        ['workflow-instances', 'elsa-studio-workflow-instances-list', true],
        ['workflow-instances/:id', 'elsa-studio-workflow-instances-view'],
        ['oauth2-authorized', 'elsa-oauth2-authorized', true],
      ]
    }
  };

  async componentWillLoad() {
    this.i18next = await loadTranslations(this.culture, resources);
    await eventBus.emit(EventTypes.Dashboard.Appearing, this, this.dashboardMenu);
  }

  componentDidLoad() {
    // Listen for navigation events to trigger re-renders
    window.addEventListener('popstate', this.handleNavigation);
  }

  disconnectedCallback() {
    window.removeEventListener('popstate', this.handleNavigation);
  }

  private handleNavigation = () => {
    this.currentPath = window.location.pathname;
  }

  render() {

    const logoPath = getAssetPath('./assets/logo.png');
    const basePath = this.basePath || '';
    const IntlMessage = GetIntlMessage(this.i18next);

    const menuItemsNamespace = "menuItems"

    let menuItems = (this.dashboardMenu.data != null ? this.dashboardMenu.data.menuItems : [])
      .map(([route, label]) =>
        this.i18next.exists(`${menuItemsNamespace}:${route}`) ?
          [route, this.i18next.t(`${menuItemsNamespace}:${route}`)] :
          [route, label]
      );

    let routes = this.dashboardMenu.data != null ? this.dashboardMenu.data.routes : [];

    const renderFeatureMenuItem = (item: any, basePath: string) => {
      return (<elsa-nav-link url={`${basePath}/${item[0]}`} anchorClass="elsa-text-gray-300 hover:elsa-bg-gray-700 hover:elsa-text-white elsa-px-3 elsa-py-2 elsa-rounded-md elsa-text-sm elsa-font-medium" activeClass="elsa-text-white elsa-bg-gray-900">
                <IntlMessage label={`${item[1]}`}/>
              </elsa-nav-link>)
    }

    const renderActiveView = () => {
      const path = (window.location.pathname || '').replace(/\\/g,'/');

      // Remove basePath prefix if present to get the route path
      let routePath = path;
      if (basePath && path.startsWith(basePath)) {
        routePath = path.substring(basePath.length);
      }
      // Remove leading slash for consistent matching
      routePath = routePath.replace(/^\/+/, '');

      // Match route patterns and extract parameters
      const matchRoute = (pattern: string, path: string): { matches: boolean; params: { [key: string]: string } } => {
        if (pattern === '') return { matches: path === '', params: {} };

        const patternParts = pattern.split('/');
        const pathParts = path.split('/');

        if (patternParts.length !== pathParts.length) return { matches: false, params: {} };

        const params: { [key: string]: string } = {};

        for (let i = 0; i < patternParts.length; i++) {
          // If pattern part starts with :, it's a parameter
          if (patternParts[i].startsWith(':')) {
            const paramName = patternParts[i].substring(1); // Remove the ':'
            params[paramName] = pathParts[i];
          } else {
            // Otherwise must match exactly
            if (patternParts[i] !== pathParts[i]) return { matches: false, params: {} };
          }
        }

        return { matches: true, params };
      };

      // Find matching route
      let matchResult = null;
      for (const route of routes) {
        const result = matchRoute(route[0], routePath);
        if (result.matches) {
          matchResult = { route, params: result.params };
          break;
        }
      }

      // Default to home route if no match
      if (!matchResult) {
        const homeRoute = routes.find(r => r[0] === '');
        if (homeRoute) {
          matchResult = { route: homeRoute, params: {} };
        } else {
          return null;
        }
      }

      const componentTag = matchResult.route[1];

      // Create match object compatible with @stencil-community/router MatchResults
      const match = {
        params: matchResult.params,
        path: routePath,
        url: path,
        isExact: true
      };

      return h(componentTag, {
        basePath: basePath,
        culture: this.culture,
        match: match
      });
    };

    return (
      <div class="elsa-h-screen elsa-bg-gray-100">
        <nav class="elsa-bg-gray-800">
          <div class="elsa-px-4 sm:elsa-px-6 lg:elsa-px-8">
            <div class="elsa-flex elsa-items-center elsa-justify-between elsa-h-16">
              <div class="elsa-flex elsa-items-center">
                <div class="elsa-flex-shrink-0">
      <elsa-nav-link url={`${basePath}/`}>
                    <img class="elsa-h-8 elsa-w-8" src={logoPath}
        alt="Workflow"/></elsa-nav-link>
                </div>
                <div class="hidden md:elsa-block">
                  <div class="elsa-ml-10 elsa-flex elsa-items-baseline elsa-space-x-4">
                    {menuItems.map(item => renderFeatureMenuItem(item, basePath))}
                  </div>
                </div>
              </div>
              <elsa-user-context-menu></elsa-user-context-menu>
            </div>
          </div>

        </nav>
        <main>
          {renderActiveView()}
        </main>

      </div>
    );
  }
}
Tunnel.injectProps(ElsaStudioDashboard, ['culture', 'basePath']);

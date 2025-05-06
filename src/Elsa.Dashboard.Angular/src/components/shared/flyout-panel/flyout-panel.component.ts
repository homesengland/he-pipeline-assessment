import { Component, ContentChildren, QueryList, AfterContentInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TabHeaderComponent } from '../tab-header/tab-header.component';
import { TabContentComponent } from '../tab-content/tab-content.component';

@Component({
  selector: 'elsa-flyout-panel',
  standalone: false,
  templateUrl: 'flyout-panel.component.html',
})
export class FlyoutPanelComponent implements AfterContentInit {
  @ContentChildren(TabHeaderComponent) tabHeaders: QueryList<TabHeaderComponent>;
  @ContentChildren(TabContentComponent) tabContents: QueryList<TabContentComponent>;

  activeTab: string = '';
  tabs: Array<{ id: string; header: TabHeaderComponent; content: TabContentComponent }> = [];

  ngAfterContentInit() {
    this.updateTabs();

    if (this.tabs.length > 0) {
      this.activeTab = this.tabs[0].id;
    }

    // Re-check tabs when content changes
    this.tabHeaders.changes.subscribe(() => this.updateTabs());
    this.tabContents.changes.subscribe(() => this.updateTabs());
  }

  updateTabs() {
    this.tabs = [];

    const headers = this.tabHeaders.toArray();
    const contents = this.tabContents.toArray();

    for (const header of headers) {
      const content = contents.find(c => c.tab === header.tab);
      if (content) {
        this.tabs.push({
          id: header.tab,
          header: header,
          content: content,
        });
      }
    }
  }

  selectTab(tabId: string, focus: boolean = false) {
    this.activeTab = tabId;

    if (focus) {
      // You would typically use ViewChild and ElementRef here to focus the tab
      setTimeout(() => {
        const tabElement = document.querySelector(`[data-tab-id="${tabId}"]`) as HTMLElement;
        if (tabElement) {
          tabElement.focus();
        }
      });
    }
  }

  isTabActive(tabId: string): boolean {
    return this.activeTab === tabId;
  }
}

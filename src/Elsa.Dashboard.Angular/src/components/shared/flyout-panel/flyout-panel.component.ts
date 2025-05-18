import { Component, ContentChildren, QueryList, AfterContentInit, Input, ViewChild, ElementRef } from '@angular/core';
import { TabHeaderComponent } from '../tab-header/tab-header.component';
import { TabContentComponent } from '../tab-content/tab-content.component';
import { enter, leave } from 'el-transition';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'flyout-panel',
  standalone: true,
  templateUrl: 'flyout-panel.component.html',
  imports: [CommonModule],
})
export class FlyoutPanelComponent implements AfterContentInit {
  @Input() expandButtonPosition = 1;
  @Input() autoExpand = false;
  @Input() hidden = false;
  @Input() silent = false;
  @ViewChild('el') el!: ElementRef;

  @ContentChildren(TabHeaderComponent) tabHeaders!: QueryList<TabHeaderComponent>;
  @ContentChildren(TabContentComponent) tabContents!: QueryList<TabContentComponent>;

  activeTab: string = '';
  tabs: Array<{ id: string; header: TabHeaderComponent; content: TabContentComponent }> = [];
  expanded: boolean = false;
  currentTab: string;

  ngAfterContentInit() {
    this.setupTabsFromContentChildren();

    this.tabHeaders.changes.subscribe(() => {
      this.setupTabsFromContentChildren();
    });

    this.tabContents.changes.subscribe(() => {
      this.setupTabsFromContentChildren();
    });
  }

  ngAfterViewInit() {
    if (this.autoExpand && this.el?.nativeElement) {
      setTimeout(() => {
        this.expanded = true;
        try {
          enter(this.el.nativeElement);
        } catch (err) {
          console.error('Error auto-expanding panel:', err);
        }
      });
    }
  }

  private setupTabsFromContentChildren() {
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

    if (this.tabs.length > 0 && !this.activeTab) {
      this.selectTab(this.tabs[0].id);
    }
  }

  selectTab(tabId: string, expand = false): void {
    this.activeTab = tabId;

    this.tabs.forEach(tab => {
      tab.header.active = tab.id === tabId;
      tab.content.active = tab.id === tabId;
    });

    if (expand && !this.expanded) {
      this.expanded = true;
      if (this.el?.nativeElement) {
        try {
          enter(this.el.nativeElement);
        } catch (err) {
          console.error('Error during enter transition:', err);
        }
      }
    }
  }

  toggle = () => {
    if (this.expanded) {
      leave(this.el.nativeElement).then(() => (this.expanded = false));
    } else {
      this.expanded = true;
      enter(this.el.nativeElement);
    }
  };
}

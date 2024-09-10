/* eslint-disable @typescript-eslint/no-extraneous-class */
export const enum E_TAB_STATUS {
  default = "default",
  active = "active",
  check = "check",
}

export type TRouter = ReturnType<typeof useRouter>;
export type TRoutes = ReturnType<TRouter["getRoutes"]>;
export type TRoute = TRoutes[0];

export type TWorkflowName = "createProject" | "updateDesign";
export type TMiddlewareName =
  | "create-project-navigate"
  | "update-design-navigate";

export interface IWorkflowTab {
  label: string;
  sortIndex: number;
  routeName: string;
  status: E_TAB_STATUS;
  to: string;
}

export interface IWorkflowTabOption {
  label: string;
  name: string;
  sortIndex: number;
}

export type TWorkflowSettings = {
  [key in TWorkflowName]: IWorkflowTabOption[];
};

export default class WorkflowService {
  public static activeTabIndex: number = 0;
  public static currentWorkflowTabsCount: number = 0;

  private static workflowSettings: TWorkflowSettings = {
    createProject: [
      {
        label: "1. Setup",
        name: "create-project",
        sortIndex: 0,
      },
      {
        label: "2. Template",
        name: "create-project-choose-template",
        sortIndex: 1,
      },
      {
        label: "3. Recipients",
        name: "create-project-recipients",
        sortIndex: 2,
      },
      {
        label: "4. Customize",
        name: "create-project-customize",
        sortIndex: 3,
      },
      {
        label: "5. Approve",
        name: "create-project-approval",
        sortIndex: 4,
      },
    ],
    updateDesign: [
      {
        label: "1. Customize",
        name: "update-design-lineItemId-customize",
        sortIndex: 0,
      },
      {
        label: "2. Approve",
        name: "update-design-lineItemId-approval",
        sortIndex: 1,
      },
    ],
  };

  public static getWorkflows(router: TRouter, lineItemId: string | undefined) {
    const routeList = router.getRoutes();

    let createProjectTabs = WorkflowService.getWorkflowTabs(
      "createProject",
      routeList,
    );
    let updateDesignTabs = WorkflowService.getWorkflowTabs(
      "updateDesign",
      routeList,
    );

    createProjectTabs = WorkflowService.preformPath(
      "createProject",
      createProjectTabs,
      lineItemId,
    );
    updateDesignTabs = WorkflowService.preformPath(
      "updateDesign",
      updateDesignTabs,
      lineItemId,
    );

    return {
      createProject: createProjectTabs,
      updateDesign: updateDesignTabs,
    };
  }

  public static getCurrentWorkflow(
    workflows: { [key in TWorkflowName]: IWorkflowTab[] },
    routeName: TRoute["name"],
  ) {
    const workflowsNames: TWorkflowName[] = ["createProject", "updateDesign"];
    const workflowName = WorkflowService.getCurrentWorkflowName(
      workflows,
      workflowsNames,
      routeName,
    );

    if (workflowName) {
      return {
        currentWorkflow: workflows[workflowName],
        currentWorkflowName: workflowName,
      };
    } else {
      return {
        currentWorkflow: null,
        currentWorkflowName: null,
      };
    }
  }

  public static getTabIndex(
    workflowTabs: IWorkflowTab[],
    routeName: TRoute["name"],
  ): number | null {
    const tabIndex = workflowTabs.findIndex((tab: IWorkflowTab) => {
      return tab.routeName === routeName;
    });
    return tabIndex === -1 ? null : tabIndex;
  }

  public static changeActiveTab(
    tabList: IWorkflowTab[],
    tabIndex: number,
  ): IWorkflowTab[] {
    WorkflowService.activeTabIndex = tabIndex;
    WorkflowService.currentWorkflowTabsCount = tabList.length;
    return tabList.map(WorkflowService.setTabStatus);
  }

  private static preformPath(
    workflowName: TWorkflowName,
    tabList: IWorkflowTab[],
    lineItemId: string | undefined,
  ) {
    if (!!lineItemId === false) return tabList;

    if (workflowName === "createProject") {
      return tabList.map((tab: IWorkflowTab) => {
        tab.to += `?lineItemId=${lineItemId}`;
        return tab;
      });
    }

    if (workflowName === "updateDesign") {
      return tabList.map((tab: IWorkflowTab) => {
        tab.to = tab.to.replace(":lineItemId()", lineItemId);
        return tab;
      });
    }

    return tabList;
  }

  private static getCurrentWorkflowName(
    workflows: { [key in TWorkflowName]: IWorkflowTab[] },
    workflowsNames: TWorkflowName[],
    routeName: TRoute["name"],
  ): TWorkflowName | undefined {
    return workflowsNames.find((workflowsName) => {
      return WorkflowService.tabListContainsRouteName(
        workflows[workflowsName],
        routeName,
      );
    });
  }

  private static setTabStatus(
    tab: IWorkflowTab,
    tabIndex: number,
  ): IWorkflowTab {
    if (tabIndex === WorkflowService.activeTabIndex) {
      tab.status = E_TAB_STATUS.active;
      return tab;
    } else if (tabIndex < WorkflowService.activeTabIndex) {
      tab.status = E_TAB_STATUS.check;
      return tab;
    } else {
      tab.status = E_TAB_STATUS.default;
      return tab;
    }
  }

  private static tabListContainsRouteName(
    workflowTabs: IWorkflowTab[],
    routeName: TRoute["name"],
  ): boolean {
    return workflowTabs.some((tab: IWorkflowTab) => {
      return tab.routeName === routeName;
    });
  }

  private static getWorkflowTabs(
    workflowName: TWorkflowName,
    routeList: TRoutes,
  ) {
    const middlewareName: TMiddlewareName =
      WorkflowService.getWorkflowMiddlewareName(workflowName);

    const filteredRoutes = routeList.filter((route) =>
      WorkflowService.pathIsCorrespondsMiddleware(route, middlewareName),
    );

    const tabs: IWorkflowTab[] = filteredRoutes.map((route) =>
      WorkflowService.getTab(route, workflowName),
    );

    const filteredTabs: IWorkflowTab[] = tabs.filter((tab) => !!tab.label);

    const sortedTabs: IWorkflowTab[] = filteredTabs.sort(
      WorkflowService.sortConditionTabs,
    );

    return sortedTabs;
  }

  private static sortConditionTabs(
    tabA: IWorkflowTab,
    tabB: IWorkflowTab,
  ): number {
    if (tabA.sortIndex < tabB.sortIndex) return -1;
    if (tabA.sortIndex > tabB.sortIndex) return 1;
    return 0;
  }

  private static getWorkflowMiddlewareName(
    workflowName: TWorkflowName,
  ): TMiddlewareName {
    switch (workflowName) {
      case "createProject":
        return "create-project-navigate";
      case "updateDesign":
        return "update-design-navigate";
    }
  }

  private static getTab(route: TRoute, workflowName: TWorkflowName) {
    const workflowSetting = WorkflowService.workflowSettings[workflowName];
    const tabSetting = workflowSetting.find(
      (setting) => setting.name === route.name,
    );

    const tab: IWorkflowTab = {
      status: E_TAB_STATUS.default,
      label: tabSetting?.label || "",
      sortIndex: tabSetting?.sortIndex || 0,
      routeName: tabSetting?.name || "",
      to: route.path,
    };
    return tab;
  }

  private static pathIsCorrespondsMiddleware(
    route: TRoute,
    middlewareName: TMiddlewareName,
  ) {
    if (!!route?.meta?.middleware?.length === false) return false;

    const middlewareList: string[] = route.meta.middleware as string[];

    if (middlewareList.length) {
      return middlewareList[0] === middlewareName;
    } else {
      return false;
    }
  }
}

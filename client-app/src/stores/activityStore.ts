import { makeAutoObservable, runInAction } from "mobx";
import { Activity } from "../app/models/activity";
import agent from "../app/api/agent";
import { v4 as uuid } from "uuid";

export default class ActivityStore {
  //activities: Activity[] = [];
  activityRegistry = new Map<string, Activity>();
  selectedActivity: Activity | undefined = undefined;
  editMode = false;
  loading = false;
  loadingInitial = false;

  constructor() {
    makeAutoObservable(this);
  }

  get activitiesByDate() {
    return Array.from(this.activityRegistry.values()).sort(
      (a, b) => Date.parse(a.date) - Date.parse(b.date)
    );
  }

  get groupedActivities() {
    //The reduce function will take an array of objects 
    //(which we get from Object.entries) and reduce it to a single value - in this case the date string. 
    //From there we use the date string as a 'key' 
    //and then the values for this 'key' will be an array of activities that take place on that date.
    return Object.entries(
      this.activitiesByDate.reduce((activities, activity) => {
        const date = activity.date;
        activities[date] = activities[date]
          ? [...activities[date], activity]
          : [activity];
        return activities;
      }, {} as { [key: string]: Activity[] })
    );
  }

  loadActivities = async () => {
    this.setLoadingInitial(true);
    try {
      const activities = await agent.Activities.list();
      activities.forEach((activity) => {
        this.setActivity(activity);
      });
      this.setLoadingInitial(false);
    } catch (error) {
      console.log(error);
      this.setLoadingInitial(false);
    }
  };

  loadActivity = async (id: string) => {
    let activity = this.getActivity(id);
    if (activity) {
      this.selectedActivity = activity;
      return activity;
    } else {
      this.setLoadingInitial(true);
      try {
        activity = await agent.Activities.details(id);
        this.setActivity(activity);
        runInAction(() => {
          this.selectedActivity = activity;
        });
        this.setLoadingInitial(false);
        return activity;
      } catch (error) {
        console.log(error);
        this.setLoadingInitial(false);
      }
    }
  };

  private setActivity = (activity: Activity) => {
    // splits date string so it gives only date without time
    activity.date = activity.date.split("T")[0];
    //this.activities.push(activity); - for normal array not map
    this.activityRegistry.set(activity.id, activity);
  };

  private getActivity = (id: string) => {
    return this.activityRegistry.get(id);
  };

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  // METHODS USED WHEN THERE WAS NO ROUTING
  // selectActivity = (id: string) => {
  //   this.selectedActivity = this.activityRegistry.get(id);
  // };

  // cancelSelectedActivity = () => {
  //   this.selectedActivity = undefined;
  // };

  // // id is optional because if we create activity we dont need it
  // openForm = (id?: string) => {
  //   id ? this.selectActivity(id) : this.cancelSelectedActivity();
  //   this.editMode = true;
  // };

  // closeForm = () => {
  //   this.editMode = false;
  // };

  createActivity = async (activity: Activity) => {
    this.loading = true;
    activity.id = uuid();
    try {
      await agent.Activities.create(activity);
      runInAction(() => {
        this.activityRegistry.set(activity.id, activity);
        this.selectedActivity = activity;
        this.editMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  updateActivity = async (activity: Activity) => {
    this.loading = true;
    try {
      await agent.Activities.update(activity);
      runInAction(() => {
        // if id exists then replace activity with updated one
        this.activityRegistry.set(activity.id, activity);
        this.editMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  deleteActivity = async (id: string) => {
    this.loading = true;
    try {
      await agent.Activities.delete(id);
      runInAction(() => {
        //deletes the activity
        this.activityRegistry.delete(id);
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };
}

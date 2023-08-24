import { makeAutoObservable, runInAction } from "mobx";
import { User, UserFormValues } from "../app/models/user";
import agent from "../app/api/agent";
import { store } from "./store";
import { router } from "../app/router/Routes";

export default class UserStore {
  user: User | null = null;

  constructor() {
    makeAutoObservable(this)
  }

  get isLoggedIn() {
    // casts to boolean
    return !!this.user;
  }

  login = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.login(creds);
      store.commonStore.setToken(user.token);
      runInAction(() => {
        this.user = user;
      })
      router.navigate('/activities')
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  }

  register = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.register(creds);
      store.commonStore.setToken(user.token);
      runInAction(() => {
        this.user = user;
      })
      router.navigate('/activities')
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  }

  logout = () => {
    store.commonStore.setToken(null);
    //localStorage.removeItem('jwt');
    this.user = null;
    router.navigate('/');
  }

  getUSer = async () => {
    try {
      const user = await agent.Account.current();
      runInAction(() => this.user = user);
    } catch (error) {
      console.log(error);
    }
  }
}
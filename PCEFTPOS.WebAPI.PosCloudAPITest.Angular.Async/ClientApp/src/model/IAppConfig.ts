export interface IAppConfig {
  apiServer: {
    uri: string;
  };
  application: {
    timer: number;
    defaultAmount: string;
    wait: number;
  };
}

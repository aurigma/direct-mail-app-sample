export class ApiClientConfiguration {
    apiUrl: string;
}

export class ApiClientBase {
    constructor(private configuration: ApiClientConfiguration) {}

    protected async transformOptions(options: any): Promise<any> {

        options = {...options, transformResponse: (res) => res, responseType: 'json'};

        return options;
    }

    protected getBaseUrl(defultUrl: string) {
        return this.configuration.apiUrl;
    }

    protected transformResult(url: string, res: any, cb: (res: any) => Promise<any>): Promise<any> {
        return cb(res);
    }
}

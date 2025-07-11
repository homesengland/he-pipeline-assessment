import axios from "axios";
import state from "../stores/store";
export async function CreateClient(auth0, serverUrl) {
    const token = await auth0.getTokenSilently();
    let config;
    if (!!token) {
        config = {
            baseURL: serverUrl,
            headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': `application/json; charset=UTF-8` }
        };
    }
    else {
        config = {
            baseURL: serverUrl,
            headers: { 'Content-Type': `application/json; charset=UTF-8` },
        };
    }
    const httpClient = axios.create(config);
    return httpClient;
}
export function GetAuth0Options() {
    let auth0Params = {
        audience: state.audience,
    };
    let auth0Options = {
        authorizationParams: auth0Params,
        clientId: state.clientId,
        domain: state.domain,
        useRefreshTokens: state.useRefreshToken,
        useRefreshTokensFallback: state.useRefreshTokenFallback,
    };
    return auth0Options;
}

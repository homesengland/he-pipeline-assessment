import state from "../stores/store";
export async function CreateClient(auth0, serverUrl) {
    const token = await auth0.getTokenSilently();
    const defaultHeaders = { 'Content-Type': 'application/json; charset=UTF-8' };
    if (token)
        defaultHeaders['Authorization'] = `Bearer ${token}`;
    const fetchWithAuth = (input, init = {}) => {
        const url = input.startsWith('http') ? input : `${serverUrl}${input}`;
        const headers = Object.assign(Object.assign({}, defaultHeaders), (init.headers || {}));
        return fetch(url, Object.assign(Object.assign({}, init), { headers }));
    };
    return fetchWithAuth;
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

//API Call to get Properties of Custom Activity child Elements.  I.e. A Question Screen's Question Property.
//Need to translate into classes that can be injested by Elsa.
//Set up Common Models Folder between here, and SRC?

export async function GetCustomActivityProperties(serverUrl) {

  serverUrl = serverUrl += '/activities/properties'
  console.log("RequestUrl", serverUrl);
  const headers = new Headers();
  headers.append("Content-Disposition", "inline");
  const request = new Request(serverUrl, { method: 'GET', headers: headers });
  const activities;
  fetch(request)
    .then((response) => console.log("Response", response))
    .then((response) => activities = response.data)
    .catch((e) => console.log(e));

  return activities;
}

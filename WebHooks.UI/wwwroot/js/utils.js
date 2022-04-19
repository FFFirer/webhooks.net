/**获取页面上的XSRF TOKEN */
const getRequestVerificationToken = function ()
{
    let antiInput = document.querySelector("input[name=__RequestVerificationToken]");

    if (antiInput)
    {
        return antiInput.value;
    }
}

/**
 * 向jquery ajax请求配置添加XSRF Token
 * @param {object} ajaxConfig
 */
const addRequestVerificationToken = function (ajaxConfig)
{
    let token = getRequestVerificationToken();
    ajaxConfig["header"]["RequestVerificationToken"] = token;
}

/**
 * 获取当前页面handler所对应的路径
 * @param {object} handler
 * @param {string} url
 */
const getUrl = function (handler)
{
    return `?handler=${handler}`;
}

export {
    getRequestVerificationToken,
    addRequestVerificationToken,
    getUrl
}
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

/**
 * ajax异步请求
 * @param {string} handler 处理程序名称
 * @param {FormData} formData 表单数据
 * @param {Function} onSuccess 成功时的操作
 * @param {Function} onError 错误时的操作
 */
const ajaxRequest = function (handler, formData, onSuccess, onError)
{
    $.ajax({
        type: "POST",
        url: getUrl(handler),
        headers: {
            "RequestVerificationToken": getRequestVerificationToken()
        },
        data: formData,
        processData: false,
        contentType: false,
        success: function ()
        {
            if (onSuccess)
            {
                onSuccess();
            }
        },
        error: function ()
        {
            if (onError)
            {
                onError()
            }
        }
    })
}

export
{
    getRequestVerificationToken,
    addRequestVerificationToken,
    getUrl,
    ajaxRequest
}
/**
 * https://davidwalsh.name/javascript-loader
 * */
var ResourceLoader = (function () {
    function _load(type) {
        return function (url) {
            return new Promise((resolve, reject) => {
                var element = document.createElement(type);
                var parent = 'body';
                var attr = 'src';

                element.onload = () => {
                    resolve(url);
                };

                element.onerror = () => {
                    reject(url);
                };

                switch (type) {
                    case 'script':
                        element.async = true;
                        break;
                    case 'link':
                        element.type = 'text/css';
                        element.rel = 'stylesheet';
                        attr = 'href';
                        parent = 'head';
                        break;
                }

                element[attr] = url;
                document[parent].appendChild(element);
            });
        };
    }

    return {
        css: _load('link'),
        js: _load('script'),
        img: _load('img')
    }
})();
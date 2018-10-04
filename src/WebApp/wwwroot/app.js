var app = (function () {
    var self = this;

    var private = {};

    private.const = {
        screenIdKey: 'ogyke-screenid'
    };

    private.config = {
        screenId: null
    };

    private.guidNew = function () {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    private.plugOn = false;
    private.plugOnToScreenIdShort = null;


    self.config = {
        screenId: null
    };

    self.setScreenId = function (id) {
        //if (!storeEnable()) {
        //    console.error('Store Enable: false');
        //    return false;
        //}

        if (!self.existsScreenId()) {
            //sessionStorage.setItem(private.const.screenIdKey, id);
            private.config.screenId = id;
        }
    };

    function storeEnable() {
        if (sessionStorage) {
            return true;
        }
        return false;
    };

    self.getScreenId = function () {
        //if (!storeEnable) {
        //    console.error('Store Enable: false');
        //    return null;
        //}
        //return sessionStorage.getItem(private.const.screenIdKey
        return private.config.screenId;
    };

    self.existsScreenId = function () {
        //var id = sessionStorage.getItem(private.const.screenIdKey);
        var id = private.config.screenId;
        if (typeof id != 'undefined' && id != null && id !== 'undefined' && id.length) {
            return true;
        }
        return false;
    };

    self.init = function () {
        var id = self.getScreenId();
        if (id == 'undefined' || id == null) {
            var newId = private.guidNew();
            self.setScreenId(newId);
        }
    }

    self.getShortScreenId = function () {
        return self.getScreenId().toString().substring(self.getScreenId().length - 4);
    }

    self.uiRefresh = function () {
        $('.shortScreenId').html(self.getShortScreenId());
    }

    self.setPlugOn = function (toScreenIdShort) {
        private.plugOnToScreenIdShort = toScreenIdShort;
        private.plugOn = true;
    }

    self.isPlugOn = function () {
        return private.plugOn;
    }

    self.getPlugOnToScreenIdShort = function () {
        return private.plugOnToScreenIdShort;
    }

    self.setPlugOff = function () {
        private.plugOnToScreenIdShort = null;
        private.plugOn = false;
    }

    self.message = function (point, msg) {

        if ('undefined' != typeof msg && msg != null) {
            console.log(msg);
        }

        if ('undefined' != typeof point && point != null) {
            console.log(point);
        }
    }

    return self;
})();




mergeInto(LibraryManager.library, {
    // Функция для определения типа устройства
    DetectDevice: function () {
        var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
        if (isMobile) {
            return 0; // Мобильное устройство
        } else {
            return 1; // Компьютер
        }
    },

    // Функция для получения языка браузера
    GetBrowserLanguage: function () {
        var language = navigator.language || navigator.userLanguage;
        var buffer = _malloc((language.length + 1) * 2); // Выделяем память для строки
        stringToUTF16(language, buffer, (language.length + 1) * 2); // Копируем строку в буфер
        return buffer; // Возвращаем указатель на строку
    },

    // Функция для инициализации обработчиков видимости страницы
    InitVisibilityHandlers: function () {
        function handleVisibilityChange() {
            if (document.hidden) {
                // Страница скрыта (вкладка неактивна или браузер свернут)
                if (typeof unityInstance !== 'undefined') {
                    unityInstance.SendMessage('GamePauseHandler', 'OnPageHidden');
                }
            } else {
                // Страница снова видна
                if (typeof unityInstance !== 'undefined') {
                    unityInstance.SendMessage('GamePauseHandler', 'OnPageVisible');
                }
            }
        }

        // Подписываемся на событие изменения видимости страницы
        document.addEventListener("visibilitychange", handleVisibilityChange);
    }
});
var MetalMonkeyEngine = MetalMonkeyEngine || {};
MetalMonkeyEngine.addClassByTag = function (tag, index, class_name) {
    document.getElementsByTagName(tag)[index].classList.add(class_name);
}

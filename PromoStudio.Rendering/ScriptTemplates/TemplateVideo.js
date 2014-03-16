var project = "{{PROJECTPATH}}",
    swapItems = { {SWAPS };},
outputPath = "{{OUTPUTPATH}}",
    renderComp = "{{RENDERCOMP}}",
    renderStart = { {RENDERSTART };},
renderDuration = { {RENDERDURATION };},
renderItemTemplate = "{{RENDERTEMPLATE}}",
    renderItem, swapItem, item, layer, i, j, k;

// Open the project
item = app.open(File(project));
if (typeof (item) !== "undefined" && item !== null) {
    project = item;
}

for (i = 0; i < swapItems.length; i++) {
    swapItem = swapItems[i];
    for (j = 1; j <= project.items.length; j++) {
        item = project.items[j];
        if (swapItem.type === "Footage") {
            // Perform footage swaps
            if (item.typeName !== swapItem.type || item.name !== swapItem.comp) {
                continue;
            }
            item.replace(File(swapItem.file));
        } else if (swapItem.type === "Text" && item.typeName === "Composition") {
            // Perform text swaps
            for (k = 1; k < item.layers.length; k++) {
                layer = item.layer(k);
                if (layer.matchName !== "ADBE Text Layer" || layer.name !== swapItem.layer) {
                    continue;
                }
                layer.property("Source Text").setValue(swapItem.text);
            }
        }
    }
}

// Start the render
for (i = 1; i <= project.items.length; i++) {
    item = project.items[i];
    if (item.typeName !== "Composition") {
        continue;
    }
    if (item.name === renderComp) {
        if (renderStart !== null) {
            item.workAreaStart = renderStart;
        }
        if (renderDuration !== null) {
            item.workAreaDuration = renderDuration;
        }
        renderItem = project.renderQueue.items.add(item);
        renderItem = renderItem.outputModule(1);
        renderItem.file = new File(outputPath);
        renderItem.includeSourceXMP = false;
        renderItem.applyTemplate(renderItemTemplate);
        break;
    }
}
project.renderQueue.render();

// Close the project
project.close(CloseOptions.DO_NOT_SAVE_CHANGES);
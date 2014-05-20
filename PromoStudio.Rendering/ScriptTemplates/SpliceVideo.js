var project = "{{PROJECTPATH}}",
    video = {{VIDEO}},
    audio = {{AUDIO}},
    renderComp = "{{RENDERCOMP}}",
    outputPath = "{{OUTPUTPATH}}",
    renderItemTemplate = "{{RENDERTEMPLATE}}",
    videoDuration = 0,
    audioDuration = 0,
    startTime = 0,
    item, layer, renderItem, i, vidScale, vidW, vidH, compW, compH, scale, audioLevel, gain;

// Open the project
item = app.open(File(project));
if (typeof (item) !== "undefined" && item !== null) {
    project = item;
}

// Get the composition	
for (i = 1; i <= project.items.length; i++) {
    item = project.items[i];
    if (item.typeName === "Composition" && item.name === renderComp) {
        renderComp = item;
        compW = renderComp.width;
        compH = renderComp.height;
        break;
    }
}

// Import the video files into adjacent layers (in order)
for (i = 0; i < video.length; i++) {
    item = project.importFile(new ImportOptions(File(video[i].file)));
    vidW = item.width;
    vidH = item.height;

    layer = renderComp.layers.add(item, item.duration);
    layer.startTime = startTime;
    layer.inPoint = startTime;
    layer.outPoint = startTime + item.duration;

    // Scale the video to fit
    scale = Math.max(compW / vidW, compH / vidH);
    scale = [scale * 100, scale * 100];
    vidScale = layer.property("Scale");
    vidScale.setValue(scale);

    // Set audio enable
    layer.audioEnabled = video[i].includeAudio;

    startTime += item.duration;
    videoDuration += item.duration;
    video[i] = layer;
}

// Import the audio files into stacked layers
for (i = 0; i < audio.length; i++) {
    item = project.importFile(new ImportOptions(File(audio[i].file)));
    layer = renderComp.layers.add(item);
    layer.startTime = 0;
    layer.inPoint = 0;
    layer.outPoint = item.duration;
    layer.audioEnabled = true;

    // Set Audio gain
    gain = audio[i].gainAdjust;
    audioLevel = layer.property("Audio Levels");
    audioLevel.setValue([gain, gain]);

    audioDuration += item.duration;
    audio[i] = layer;
}

// Set comp duration
renderComp.duration = videoDuration;

// Start the render
renderItem = project.renderQueue.items.add(renderComp);
renderItem = renderItem.outputModule(1);
renderItem.file = new File(outputPath);
renderItem.includeSourceXMP = false;
renderItem.applyTemplate(renderItemTemplate);
project.renderQueue.render();

// Close the project
project.close(CloseOptions.DO_NOT_SAVE_CHANGES);
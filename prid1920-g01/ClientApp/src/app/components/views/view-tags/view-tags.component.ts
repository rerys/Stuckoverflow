import { Input, Component } from "@angular/core";
import { Tag } from "src/app/models/tag";

@Component({
    selector: 'tags',
    templateUrl: './view-tags.component.html',
    styleUrls: ['./view-tags.component.css']
    
})

export class TagsComponent{
    @Input() data: Tag;

    constructor(){ 

    }
}
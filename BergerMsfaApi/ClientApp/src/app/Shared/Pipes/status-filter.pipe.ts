import { Pipe, PipeTransform } from '@angular/core';
import { PlanStatus } from '../Enums/PlanStatus';




@Pipe({ name: 'msfaStatus' })
export class StatusPipe implements PipeTransform {
    transform(value: number): string {
        debugger;
        var val = Number(value) ? PlanStatus[value] : undefined;
        return val;
    }
}

//@Pipe({ name: 'msfaPlanStatus' })
//export class PlanStatusPipe implements PipeTransform {
//    transform(value: number): string {
//        debu
//        return Number(value) ? PlanStatus[value] : undefined;
//    }
//}
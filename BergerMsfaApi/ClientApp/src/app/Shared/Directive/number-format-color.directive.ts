import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { Directive,ElementRef,Input,OnInit,Renderer2} from '@angular/core';

@Directive({
    selector: '[numberFormatColor]',
    providers: [
        CurrencyPipe,
        DecimalPipe
    ]
})

export class NumberFormatColorDirective implements OnInit {
    @Input() numberFormatColor: any;
    element: any;

    constructor(
        private renderer: Renderer2, 
        private elementRef: ElementRef, 
        private currencyPipe: CurrencyPipe, 
        private decimalPipe: DecimalPipe) {
        this.element = this.elementRef.nativeElement;
    }

    ngOnInit() {
        this.updateValueFormat();
    }

    updateValueFormat() {
        const negativeNumberFormatColorStyles = {
            // 'background-color': 'red',
            // 'color': 'white',
            // 'padding': '3px',
            // 'border-radius': '5px',
            'color': 'red',
        };
        
        let value = this.numberFormatColor || 0;
        const formatValue = this.decimalPipe.transform(value, '1.0-2', 'en-US');

        if (value < 0) {
            Object.keys(negativeNumberFormatColorStyles).forEach(newStyle => {
                this.renderer.setStyle(
                    this.element, `${newStyle}`, negativeNumberFormatColorStyles[newStyle]
                );
            });
        }

        this.renderer.setProperty(
            this.element, `innerText`, formatValue
        );
    }
}
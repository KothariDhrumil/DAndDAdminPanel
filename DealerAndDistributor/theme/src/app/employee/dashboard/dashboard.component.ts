import { Component, OnInit, ViewChild } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTooltip,
  ApexYAxis,
  ApexPlotOptions,
  ApexStroke,
  ApexLegend,
  ApexNonAxisChartSeries,
  ApexFill,
  ApexGrid,
  ApexResponsive,
  NgApexchartsModule,
} from 'ng-apexcharts';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { NgScrollbar } from 'ngx-scrollbar';
import { MatButtonModule } from '@angular/material/button';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatCardModule } from '@angular/material/card';
import { EmpStatusComponent } from '@shared/components/emp-status/emp-status.component';
import { TableCardComponent } from '@shared/components/table-card/table-card.component';
import { TodoWidgetComponent } from '@shared/components/todo-widget/todo-widget.component';

interface Todo {
  title: string;
  done: boolean;
  priority: 'Low' | 'Normal' | 'High';
}

export type chartOptions = {
  series: ApexAxisChartSeries;
  radialseries: ApexNonAxisChartSeries;
  series2: ApexNonAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  yaxis: ApexYAxis;
  xaxis: ApexXAxis;
  fill: ApexFill;
  tooltip: ApexTooltip;
  grid: ApexGrid;
  stroke: ApexStroke;
  legend: ApexLegend;
  colors: string[];
  responsive: ApexResponsive[];
  labels: string[];
};

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
    imports: [
        BreadcrumbComponent,
        NgApexchartsModule,
        MatButtonModule,
        NgScrollbar,
        MatCardModule,
        MatIconModule,
        MatCheckboxModule,
        MatTooltipModule,
        EmpStatusComponent,
        TableCardComponent,
        TodoWidgetComponent,
    ]
})
export class DashboardComponent implements OnInit {
  @ViewChild('chart')
  chart!: ChartComponent;
  public barChartOptions!: Partial<chartOptions>;
  public radialChartOptions!: Partial<chartOptions>;
  public gaugeChartOptions!: Partial<chartOptions>;
  public stackBarChart!: Partial<chartOptions>;
  constructor() {
    // code here
  }

  // TODO start
  tasks: Todo[] = [
    { title: 'Buy groceries', done: false, priority: 'Normal' },
    { title: 'Finish project report', done: false, priority: 'High' },
    { title: 'Clean the house', done: true, priority: 'Low' },
    { title: 'Call the bank', done: false, priority: 'Normal' },
    { title: 'Read a book', done: false, priority: 'Low' },
    { title: 'Schedule doctor appointment', done: false, priority: 'High' },
    { title: 'Prepare for presentation', done: false, priority: 'Normal' },
    { title: 'Exercise for 30 minutes', done: false, priority: 'Normal' },
    { title: 'Finish laundry', done: true, priority: 'Low' },
    { title: 'Write blog post', done: false, priority: 'High' },
    { title: 'Organize workspace', done: false, priority: 'Normal' },
    { title: 'Plan weekend trip', done: false, priority: 'High' },
    { title: 'Buy gifts for friends', done: false, priority: 'Low' },
  ];

  onTodoToggled(todo: any) {
    console.log('Todo toggled:', todo);
  }

  onTodosUpdated(updatedTodos: any[]) {
    console.log('Todos updated:', updatedTodos);
  }
  // TODO end

  ngOnInit() {
    this.chart1();
    this.chart2();
    this.gaugechart();
    this.stackChart();
  }
  private chart1() {
    this.barChartOptions = {
      series: [
        {
          name: 'Work Hours',
          data: [6.3, 5.5, 4.1, 6.7, 2.2, 4.3],
        },
        {
          name: 'Break Hours',
          data: [1.3, 2.3, 2.0, 0.8, 1.3, 2.7],
        },
      ],
      chart: {
        type: 'bar',
        height: 350,
        stacked: true,
        stackType: '100%',
        foreColor: '#9aa0ac',
      },
      colors: ['#674EC9', '#C1C1C1'],
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '35%',
        },
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            legend: {
              position: 'bottom',
              offsetX: -10,
              offsetY: 0,
            },
          },
        },
      ],
      xaxis: {
        categories: [
          'Monday',
          'Tuesday',
          'Wednesday',
          'Thursday',
          'Friday',
          'Saturday',
        ],
      },
      fill: {
        opacity: 1,
      },
      legend: {
        position: 'bottom',
        offsetX: 0,
        offsetY: 0,
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }
  private chart2() {
    this.radialChartOptions = {
      radialseries: [44, 55, 67],
      chart: {
        height: 290,
        type: 'radialBar',
      },
      plotOptions: {
        radialBar: {
          dataLabels: {
            name: {
              fontSize: '22px',
            },
            value: {
              fontSize: '16px',
            },
            total: {
              show: true,
              label: 'Total',
              formatter: function () {
                return '52%';
              },
            },
          },
        },
      },
      labels: ['Project 1', 'Project 2', 'Project 3'],
    };
  }
  private gaugechart() {
    this.gaugeChartOptions = {
      series2: [72],
      chart: {
        height: 310,
        type: 'radialBar',
        offsetY: -10,
      },
      plotOptions: {
        radialBar: {
          startAngle: -135,
          endAngle: 135,
          dataLabels: {
            name: {
              fontSize: '22px',
              fontWeight: 600,
              color: '#6777EF',
              offsetY: 120,
            },
            value: {
              offsetY: 76,
              fontSize: '22px',
              color: '#9aa0ac',
              formatter: function (val) {
                return val + '%';
              },
            },
          },
        },
      },
      fill: {
        type: 'gradient',
        gradient: {
          shade: 'dark',
          shadeIntensity: 0.15,
          inverseColors: false,
          opacityFrom: 1,
          opacityTo: 1,
        },
      },
      stroke: {
        dashArray: 4,
      },
      labels: ['Closed Ticket'],
    };
  }
  private stackChart() {
    this.stackBarChart = {
      series: [
        {
          name: 'Project 1',
          data: [44, 55, 41, 67, 22, 43],
        },
        {
          name: 'Project 2',
          data: [13, 23, 20, 8, 13, 27],
        },
        {
          name: 'Project 3',
          data: [11, 17, 15, 15, 21, 14],
        },
        {
          name: 'Project 4',
          data: [21, 7, 25, 13, 22, 8],
        },
      ],
      chart: {
        type: 'bar',
        height: 300,
        foreColor: '#9aa0ac',
        stacked: true,
        toolbar: {
          show: false,
        },
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            legend: {
              position: 'bottom',
              offsetX: -10,
              offsetY: 0,
            },
          },
        },
      ],
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '30%',
        },
      },
      dataLabels: {
        enabled: false,
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      xaxis: {
        type: 'category',
        categories: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
      },
      legend: {
        show: false,
      },
      fill: {
        opacity: 1,
        colors: ['#F0457D', '#704DAD', '#FFC107', '#a5a5a5'],
      },
    };
  }

  myTaskData = [
    {
      task: 'Task A',
      status: 'Not Started',
      manager: 'Jay Soni',
      progress: 62,
      documents: 'assets/images/user/user1.jpg',
      detailsLink: '#/admin/task/details',
    },
    {
      task: 'Task B',
      status: 'Completed',
      manager: 'Sarah Smith',
      progress: 40,
      documents: 'assets/images/user/user2.jpg',
      detailsLink: '#/admin/task/details',
    },
    {
      task: 'Task C',
      status: 'In Progress',
      manager: 'Megha Trivedi',
      progress: 61,
      documents: 'assets/images/user/user3.jpg',
      detailsLink: '#/admin/task/details',
    },
    {
      task: 'Task D',
      status: 'Pending',
      manager: 'Jacob Ryan',
      progress: 95,
      documents: 'assets/images/user/user4.jpg',
      detailsLink: '#/admin/task/details',
    },
    {
      task: 'Task E',
      status: 'In Progress',
      manager: 'Airi Satou',
      progress: 87,
      documents: 'assets/images/user/user5.jpg',
      detailsLink: '#/admin/task/details',
    },
    {
      task: 'Task A',
      status: 'Not Started',
      manager: 'Angelica Ramos',
      progress: 62,
      documents: 'assets/images/user/user1.jpg',
      detailsLink: '#/admin/task/details',
    },
    {
      task: 'Task B',
      status: 'Completed',
      manager: 'Ashton Cox',
      progress: 40,
      documents: 'assets/images/user/user2.jpg',
      detailsLink: '#/admin/task/details',
    },
  ];

  myTaskColumns = [
    { def: 'task', label: 'Task', type: 'text' },
    { def: 'status', label: 'Status', type: 'badge' },
    { def: 'manager', label: 'Manager', type: 'text' },
    { def: 'progress', label: 'Progress', type: 'progressBar' },
    { def: 'documents', label: 'Documents', type: 'file' },
    { def: 'detailsLink', label: 'Details', type: 'button' },
  ];
}
